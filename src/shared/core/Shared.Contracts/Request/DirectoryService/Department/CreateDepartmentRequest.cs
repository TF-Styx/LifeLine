namespace Shared.Contracts.Request.DirectoryService.Department
{
    public sealed record CreateDepartmentRequest
        (
            string Name,
            string Description,
            List<CreateDepartmentPositionRequestData> Positions,
            CreateDepartmentAddressRequestData Address
        );

    public sealed record CreateDepartmentAddressRequestData(string PostalCode, string Region, string City, string Street, string Building, string? Apartment);

    public sealed record CreateDepartmentPositionRequestData(string Name, string Description);
}
