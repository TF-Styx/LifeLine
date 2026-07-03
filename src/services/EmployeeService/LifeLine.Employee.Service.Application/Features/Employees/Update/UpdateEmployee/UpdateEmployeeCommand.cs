using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using Shared.Api.Application.Validators.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployee
{
    public sealed record UpdateEmployeeCommand(Guid Id, string Surname, string Name, string? Patronymic, Guid GenderId) : IRequest<Result<Nothing>>, IHasFIO;
}
