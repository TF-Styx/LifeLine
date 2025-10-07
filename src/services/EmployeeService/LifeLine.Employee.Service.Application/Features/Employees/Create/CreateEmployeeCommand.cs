using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Create
{
    public sealed record CreateEmployeeCommand(string Surname, string Name, string Patronymic, Guid GenderId) : IRequest<Result>, IHasFIO;
}
