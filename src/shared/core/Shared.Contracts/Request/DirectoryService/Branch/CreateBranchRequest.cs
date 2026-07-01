namespace Shared.Contracts.Request.DirectoryService.Branch
{
    public sealed record CreateBranchRequest(string Name, string? Description, string Phone, string Email, string HospitalId, CreateBranchDataAddressRequest Address);

    public sealed record CreateBranchDataAddressRequest(string PostalCode, string Region, string City, string Street);
}
