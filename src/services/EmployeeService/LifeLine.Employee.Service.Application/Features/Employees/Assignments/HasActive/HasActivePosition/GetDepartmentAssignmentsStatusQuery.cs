using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.HasActive.HasActivePosition
{
    public sealed record GetPositionAssignmentsStatusQuery(Guid PositionId) : IRequest<Result>;
}
