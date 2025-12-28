using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Delete
{
    public sealed record DeleteAssignmentCommand(Guid EmployeeId, Guid AssignmentId) : IRequest<Result>;
}
