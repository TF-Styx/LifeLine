using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Shared.Contracts.Response.EmployeeService;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.CreateEmployee
{
    public sealed record CreateEmployeeCommand(string Surname, string Name, string? Patronymic, Guid GenderId) : IRequest<Result<EmployeeIdResponse>>, IHasFIO;
}
