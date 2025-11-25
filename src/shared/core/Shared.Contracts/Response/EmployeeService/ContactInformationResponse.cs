namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record ContactInformationResponse
        (
            string Id,
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
