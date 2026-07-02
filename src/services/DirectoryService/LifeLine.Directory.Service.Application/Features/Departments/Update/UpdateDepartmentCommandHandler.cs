using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Domain.ValueObjects.AddressVO;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Departments.Update
{
    public sealed class UpdateDepartmentCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository
        ) : IRequestHandler<UpdateDepartmentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.DepartmentId);

            if (department == null)
                return Error.NotFound("Запись департамента не найдена!");

            department.UpdateName(DirectoryName.Create(request.Name));
            department.UpdateDescription(!string.IsNullOrWhiteSpace(request.Description) ? Description.Create(request.Description) : null);
            department.UpdateBuilding(Building.Create(request.Building));
            department.UpdateBranchId(BranchId.Create(request.BranchId));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
