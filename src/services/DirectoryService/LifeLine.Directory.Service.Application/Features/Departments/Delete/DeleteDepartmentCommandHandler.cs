using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Employee.Service.Client.Services.Employee.Assignment;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Errors;
using Terminex.Common.Primitives;
using Terminex.Common.Results;

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
            var department = await repository.GetByIdAsync(request.Id);

            if (department == null)
                return Error.NotFound("Запись департамента не найдена!");

            var hasPosition = await context.Positions.AnyAsync(x => x.DepartmentId == department.Id && !x.IsDeleted);

            if (hasPosition)
                return new Error(AppErrors.ExistDependentData, $"У отдела - `{department.Name}`, имеются должности!");

            var hasActiveAssignmentResult = await service.HasActiveAssignmentsToDepartmentAsync(department.Id, cancellationToken);

            if (hasActiveAssignmentResult.IsFailure)
                return new Error(AppErrors.ExistDependentData, "У департамента существуют зависимые данные!");

            department.Delete();

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
