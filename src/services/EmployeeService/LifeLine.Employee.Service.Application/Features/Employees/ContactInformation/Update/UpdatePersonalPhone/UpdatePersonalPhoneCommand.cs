using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdatePersonalPhone
{
    public sealed record UpdatePersonalPhoneCommand(Guid EmployeeId, string PersonalPhone) : IRequest<Result>;
}
