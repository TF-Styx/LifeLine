using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Delete
{
    public sealed record DeleteEmployeeCommand(Guid Id) : IRequest<Result>;
}
