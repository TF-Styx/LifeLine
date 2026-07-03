using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Create
{
    public sealed record CreateContactInformationCommand
    (
        Guid EmployeeId, 
        string PersonalPhone, 
        string? CorporatePhone, 
        string PersonalEmail, 
        string? CorporateEmail, 
        CreateAddressCommandData Address
    ) : IRequest<Result<Nothing>>;

    public sealed record CreateAddressCommandData
    (
        string PostalCode, 
        string Region, 
        string City, 
        string Street, 
        string Building, 
        string? Apartment
    );
}
