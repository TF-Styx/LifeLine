using MediatR;
using Terminex.Common.Results;
using Shared.Api.Application.Validators.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Employees.CreateEmployee
{
    public sealed record CreateEmployeeCommand(string Surname, string Name, string? Patronymic, Guid GenderId) : IRequest<Result<string>>, IHasFIO;
}
