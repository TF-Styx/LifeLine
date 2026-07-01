namespace Shared.Contracts.Response.DirectoryService
{
    public sealed record BranchResponse(string Id, string Name, string? Description, string Phone, string Email, string HospitalId, BranchDataAddressResponse Address);

    public sealed record BranchDataAddressResponse(string PostalCode, string Region, string City, string Street);
}
