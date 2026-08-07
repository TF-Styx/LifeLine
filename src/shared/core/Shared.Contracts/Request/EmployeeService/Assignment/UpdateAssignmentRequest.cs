namespace Shared.Contracts.Request.EmployeeService.Assignment
{
    public sealed record UpdateAssignmentRequest
        (
            string PositionId,
            string DepartmentId,
            string BranchId,
            string? ManagerId,
            DateTime HireDate,
            DateTime? TerminationDate,
            string StatusId,
            UpdateAssignmentContractRequest Contract
        );

    public sealed record UpdateAssignmentContractRequest
        (
            string EmployeeTypeId,
            string ContractNumber,
            DateTime StartDate,
            DateTime EndDate,
            decimal Salary,
            string? BucketName,
            string? FileName
        );
}
