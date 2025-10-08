using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateCorporatePhone
{
    public sealed record UpdateCorporatePhoneCommand(Guid EmployeeId, string CorporatePhone) : IRequest<Result>;
}
