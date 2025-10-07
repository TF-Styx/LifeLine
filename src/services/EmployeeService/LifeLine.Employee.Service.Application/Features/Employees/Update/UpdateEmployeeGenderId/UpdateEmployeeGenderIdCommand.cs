using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeGenderId
{
    public sealed record UpdateEmployeeGenderIdCommand(Guid Id, Guid GenderId) : IRequest<Result>;
}
