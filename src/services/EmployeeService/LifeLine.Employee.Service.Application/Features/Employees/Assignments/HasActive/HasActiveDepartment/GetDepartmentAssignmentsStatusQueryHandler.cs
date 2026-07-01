using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Shared.Domain.ValueObjects;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.HasActive.HasActiveDepartment
{
    public sealed class GetDepartmentAssignmentsStatusQueryHandler(IEmployeeRepository repository) 
        : IRequestHandler<GetDepartmentAssignmentsStatusQuery, Result>
    {
        public async Task<Result> Handle(GetDepartmentAssignmentsStatusQuery request, CancellationToken cancellationToken)
            => await repository.HasActiveAssignmentsToDepartmentAsync
               (
                   DepartmentId.Create(request.DepartmentId),
                   cancellationToken
               );
    }
}
