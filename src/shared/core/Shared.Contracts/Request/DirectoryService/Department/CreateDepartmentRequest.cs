namespace Shared.Contracts.Request.DirectoryService.Department
{
    public sealed record CreateDepartmentRequest
        (
            string Name,
            string? Description,
            string Building,
            string BranchId
        );
}
