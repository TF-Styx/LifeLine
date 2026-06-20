namespace Shared.Contracts.Request.DirectoryService.Department
{
    public sealed record UpdateDepartmentRequest(string Name, string Description, UpdateDepartmentDataAddressRequest Address);

    public sealed record UpdateDepartmentDataAddressRequest(string PostalCode, string Region, string City, string Street, string Building);
}
