using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeSurname
{
    public sealed record UpdateEmployeeSurnameCommand(Guid Id, string Surname) : IRequest<Result>, IHasSurname;
}
