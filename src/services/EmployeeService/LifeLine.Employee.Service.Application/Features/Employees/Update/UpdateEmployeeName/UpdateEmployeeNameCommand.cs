using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeName
{
    public sealed record UpdateEmployeeNameCommand(Guid Id, string Name) : IRequest<Result>, IHasName;
}
