using MediatR;
using Terminex.Common.Results;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Departments.Create
{
    public sealed class CreateDepartmentCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository
        ) : IRequestHandler<CreateDepartmentCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = Department.Create(request.Name, request.Description, request.Building, request.BranchId);

            await repository.AddAsync(department, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return department.Id.ToString();
        }
    }
}
