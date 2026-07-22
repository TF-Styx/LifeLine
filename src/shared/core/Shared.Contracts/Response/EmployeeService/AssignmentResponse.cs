namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record AssignmentResponse
        (
            string AssignmentId,
            string EmployeeId,
            string PositionId,
            string DepartmentId,
            string BranchId,
            string? ManagerId,
            DateTime HireDate,
            DateTime? TerminationDate,
            string StatusId,
            string ContractId
        );
}
