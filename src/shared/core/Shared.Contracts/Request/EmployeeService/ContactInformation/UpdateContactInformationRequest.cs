namespace Shared.Contracts.Request.EmployeeService.ContactInformation
{
    public sealed record UpdateContactInformationRequest
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
        );
}
