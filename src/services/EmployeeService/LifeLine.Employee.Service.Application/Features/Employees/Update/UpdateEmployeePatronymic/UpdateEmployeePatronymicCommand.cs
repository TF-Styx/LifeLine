using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeePatronymic
{
    public sealed record UpdateEmployeePatronymicCommand(Guid Id, string Patronymic) : IRequest<Result>, IMayHasPatronymic;
}
