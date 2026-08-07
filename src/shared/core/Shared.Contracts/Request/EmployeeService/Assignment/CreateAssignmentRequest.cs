namespace Shared.Contracts.Request.EmployeeService.Assignment
{
    public sealed record CreateAssignmentRequest(string PositionId, string DepartmentId, string BranchId, string? ManagerId, DateTime HireDate, DateTime? TerminationDate, string StatusId, CreateAssignmentContractRequest Contract);

    public sealed record CreateAssignmentContractRequest(string EmployeeTypeId, string ContractNumber, DateTime StartDate, DateTime EndDate, decimal Salary, string BucketName, string FileName);
}
