using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Create
{
    public sealed class CreatePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            ILogger<CreatePositionCommandHandler> logger
        ) : IRequestHandler<CreatePositionCommand, Result<string>>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _repository = repository;
        private readonly ILogger<CreatePositionCommandHandler> _logger = logger;

        public async Task<Result<string>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _repository.GetByIdAsync(request.Id);

                if (department == null)
                    return Result<string>.Failure(new Error(ErrorCode.NotFound, "Запись департамента не найдена!"));

                department.AddPositions(request.Name, request.Description);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<string>.Success(department.Id.ToString());
            }
            catch (DomainException domainEX)
            {
                return Result<string>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Position!");

                return Result<string>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
