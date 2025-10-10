using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Update
{
    public sealed class UpdatePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository departmentRepository,
            ILogger<UpdatePositionCommandHandler> logger
        ) : IRequestHandler<UpdatePositionCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly ILogger<UpdatePositionCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);

                if (department == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись департамента не найдена!"));

                department.UpdatePositionName(request.PositionId, request.Name);
                department.UpdatePositionDescription(request.PositionId, request.Description);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении Position!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
