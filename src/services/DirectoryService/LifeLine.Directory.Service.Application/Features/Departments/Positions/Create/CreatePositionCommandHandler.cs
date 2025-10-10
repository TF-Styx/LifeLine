using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Application.Features.Departments.Create;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Create
{
    public sealed class CreatePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            ILogger<CreatePositionCommandHandler> logger
        ) : IRequestHandler<CreatePositionCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _repository = repository;
        private readonly ILogger<CreatePositionCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _repository.GetByIdAsync(request.Id);

                if (department == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись департамента не найдена!"));

                department.AddPositions(request.Name, request.Description);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result<Guid>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Position!");

                return Result<Guid>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
