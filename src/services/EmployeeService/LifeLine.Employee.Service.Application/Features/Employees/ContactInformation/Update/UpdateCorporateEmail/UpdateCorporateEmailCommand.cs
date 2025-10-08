using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateCorporateEmail
{
    public sealed record UpdateCorporateEmailCommand(Guid EmployeeId, string CorporateEmail) : IRequest<Result>;
}
