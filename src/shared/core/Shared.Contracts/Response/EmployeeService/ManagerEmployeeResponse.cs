namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record ManagerEmployeeResponse(Guid Id, string FIO, Guid DepartmentId, Guid PositionId);
}
