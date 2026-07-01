namespace Shared.Contracts.Request.DirectoryService.Hospital
{
    public sealed record CreateHospitalRequest(string Name, string? Description, string Phone, string Email, CreateHospitalDataAddressRequest Address);

    public sealed record CreateHospitalDataAddressRequest(string PostalCode, string Region, string City, string Street);
}
