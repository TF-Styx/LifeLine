using MediatR;
using Shared.Kernel.Errors;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Employee.Service.Client.Services.Employee.Assignment;

namespace LifeLine.Directory.Service.Application.Features.Departments.Delete
{
    public sealed class DeleteDepartmentCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            IAssignmentCheckService service
        ) : IRequestHandler<DeleteDepartmentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.DepartmentId);

            if (department == null)
                return Error.NotFound("Запись департамента не найдена!");

            var hasActiveAssignmentResult = await service.HasActiveAssignmentsToDepartmentAsync(department.Id, cancellationToken);

            if (hasActiveAssignmentResult.IsFailure)
                return new Error(AppErrors.ExistDependentData, "У департамента существуют зависимые данные!");

            repository.Remove(department);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
