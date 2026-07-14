using MediatR;
using Shared.Kernel.Errors;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Employee.Service.Client.Services.Employee.Assignment;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Delete
{
    public sealed class DeletePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            IAssignmentCheckService service
        ) : IRequestHandler<DeletePositionCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.DepartmentId);

            if (department == null)
                return Error.NotFound("Отдел не найден!");

            var positionExists = department.Positions.Any(x => x.Id == request.PositionId);

            if (!positionExists)
                return Error.NotFound("Должность не найдена в данном департаменте!");

            var hasActiveAssignmentResult = await service.HasActiveAssignmentsToPositionAsync(request.PositionId, cancellationToken);

            if (hasActiveAssignmentResult.IsFailure)
                return new Error(AppErrors.ExistDependentData, "Невозможно удалить должность: существуют активные назначения сотрудников!");

            department.DeletePosition(request.PositionId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
