using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployee
{
    public sealed record UpdateEmployeeCommand(Guid Id, string Surname, string Name, string Patronymic, Guid GenderId) : IRequest<Result>, IHasFIO;
}
