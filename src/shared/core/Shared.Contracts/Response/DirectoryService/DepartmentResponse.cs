namespace Shared.Contracts.Response.DirectoryService
{
    public sealed record DepartmentResponse(string Id, string Name, string Description, DepartmentDataAddressResponse Address);

    public sealed record DepartmentDataAddressResponse(string PostalCode, string Region, string City, string Street, string Building);
}
