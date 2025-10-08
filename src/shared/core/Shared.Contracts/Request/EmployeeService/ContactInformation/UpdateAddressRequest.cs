namespace Shared.Contracts.Request.EmployeeService.ContactInformation
{
    public sealed record UpdateAddressRequest(string PostalCode, string Region, string City, string Street, string Building, string? Apartment);
}
