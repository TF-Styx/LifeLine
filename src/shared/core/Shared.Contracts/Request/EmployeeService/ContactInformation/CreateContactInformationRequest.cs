namespace Shared.Contracts.Request.EmployeeService.ContactInformation
{
    public sealed record CreateContactInformationRequest(string PersonalPhone, string? CorporatePhone, string PersonalEmail, string? CorporateEmail, string PostalCode, string Region, string City, string Street, string Building, string? Apartment);
}
