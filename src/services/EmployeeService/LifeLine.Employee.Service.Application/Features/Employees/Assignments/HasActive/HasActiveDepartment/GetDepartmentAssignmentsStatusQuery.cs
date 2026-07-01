using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.HasActive.HasActiveDepartment
{
    public sealed record GetDepartmentAssignmentsStatusQuery(Guid DepartmentId) : IRequest<Result>;
}
