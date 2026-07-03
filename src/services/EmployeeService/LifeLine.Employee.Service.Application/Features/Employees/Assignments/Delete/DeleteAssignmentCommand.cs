using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Delete
{
    public sealed record DeleteAssignmentCommand(Guid EmployeeId, Guid AssignmentId) : IRequest<Result<Nothing>>;
}
