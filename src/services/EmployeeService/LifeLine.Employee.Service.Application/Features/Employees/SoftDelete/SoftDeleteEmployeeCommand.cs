using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.SoftDelete
{
    public sealed record SoftDeleteEmployeeCommand(Guid EmployeeId) : IRequest<Result>;
}
