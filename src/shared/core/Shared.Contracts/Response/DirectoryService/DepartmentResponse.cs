namespace Shared.Contracts.Response.DirectoryService
{
    public sealed record DepartmentResponse(string Id, string Name, string? Description, string Building, string BranchId);
}
