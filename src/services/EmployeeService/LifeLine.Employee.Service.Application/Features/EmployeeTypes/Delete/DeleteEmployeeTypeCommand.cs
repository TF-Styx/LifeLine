using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Delete
{
    public sealed record DeleteEmployeeTypeCommand(Guid Id) : IRequest<Result>;
}
