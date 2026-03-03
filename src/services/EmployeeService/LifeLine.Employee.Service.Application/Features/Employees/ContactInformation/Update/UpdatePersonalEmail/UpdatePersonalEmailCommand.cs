using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdatePersonalEmail
{
    public sealed record UpdatePersonalEmailCommand(Guid EmployeeId, string PersonalEmail) : IRequest<Result>;
}
