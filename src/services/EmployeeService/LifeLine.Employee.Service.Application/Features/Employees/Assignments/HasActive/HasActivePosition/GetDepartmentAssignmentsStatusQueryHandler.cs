using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Shared.Domain.ValueObjects;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.HasActive.HasActivePosition
{
    public sealed class GetPositionAssignmentsStatusQueryHandler
        (
            IEmployeeRepository repository 
        ) : IRequestHandler<GetPositionAssignmentsStatusQuery, Result>
    {
        public async Task<Result> Handle(GetPositionAssignmentsStatusQuery request, CancellationToken cancellationToken)
            => await repository.HasActiveAssignmentsToPositionAsync
               (
                   PositionId.Create(request.PositionId),
                   cancellationToken
               );
    }
}
