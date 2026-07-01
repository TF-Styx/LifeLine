using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Employee.Service.Client.Services.Employee.Assignment;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Errors;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Delete
{
    public sealed class DeletePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            IAssignmentCheckService service,
            ILogger<DeletePositionCommandHandler> logger
        ) : IRequestHandler<DeletePositionCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _repository = repository;
        private readonly IAssignmentCheckService _service = service;
        private readonly ILogger<DeletePositionCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _repository.GetByIdAsync(request.DepartmentId);

                if (department == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Отдел не найден!"));

                var positionExists = department.Positions.Any(x => x.Id == request.PositionId);

                if (!positionExists)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Должность не найдена в данном департаменте!"));

                var hasActiveAssignmentResult = await _service.HasActiveAssignmentsToPositionAsync(request.PositionId, cancellationToken);

                if (hasActiveAssignmentResult.IsFailure)
                    return Result.Failure(new Error(AppErrors.ExistDependentData, "Невозможно удалить должность: существуют активные назначения сотрудников!"));


                department.RemovePosition(request.PositionId);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении должности!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
