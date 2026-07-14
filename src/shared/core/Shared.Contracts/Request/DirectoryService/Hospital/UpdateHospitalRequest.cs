namespace Shared.Contracts.Request.DirectoryService.Hospital
{
    public sealed record UpdateHospitalRequest(string Name, string? Description, string Phone, string Email, UpdateHospitalDataAddressRequest Address);

    public sealed record UpdateHospitalDataAddressRequest(string PostalCode, string Region, string City, string Street, string? Building, string? Apartment);
}
