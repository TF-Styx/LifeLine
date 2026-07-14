namespace Shared.Contracts.Response.DirectoryService
{
    public sealed record HospitalResponse(string Id, string Name, string? Description, string Phone, string Email, HospitalDataAddressResponse Address);

    public sealed record HospitalDataAddressResponse(string PostalCode, string Region, string City, string Street, string? Building, string? Apartment);
}
