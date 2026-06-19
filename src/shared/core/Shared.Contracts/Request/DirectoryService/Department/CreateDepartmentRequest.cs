namespace Shared.Contracts.Request.DirectoryService.Department
{
    public sealed record CreateDepartmentRequest
        (
            string Name,
            string Description,
            CreateDepartmentAddressRequestData Address
        );

    public sealed record CreateDepartmentAddressRequestData(string PostalCode, string Region, string City, string Street, string Building);
}
