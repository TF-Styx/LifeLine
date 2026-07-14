namespace Shared.Contracts.Request.DirectoryService.Branch
{
    public sealed record UpdateBranchRequest(string Name, string? Description, string Phone, string Email, string HospitalId, UpdateBranchDataAddressRequest Address);

    public sealed record UpdateBranchDataAddressRequest(string PostalCode, string Region, string City, string Street, string? Building, string? Apartment);
}
