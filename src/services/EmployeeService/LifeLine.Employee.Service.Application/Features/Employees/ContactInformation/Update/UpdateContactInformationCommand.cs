using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update
{
    public sealed record UpdateContactInformationCommand
        (
            string Id,
            string EmployeeId,
            string PersonalPhone,
            string? CorporatePhone,
            string PersonalEmail,
            string? CorporateEmail,
            string PostalCode,
            string Region,
            string City,
            string Street,
            string Building,
            string? Apartment
        ) : IRequest<Result>;
}
