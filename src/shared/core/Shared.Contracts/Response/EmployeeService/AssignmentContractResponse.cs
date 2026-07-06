namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record AssignmentContractResponse(AssignmentContractDataResponse AssignmentsContracts);

    public sealed record AssignmentContractDataResponse(List<AssignmentDataResponse> Assignment, List<ContractDataResponse> Contract);

    public sealed record AssignmentDataResponse
        (
            string AssignmentId,
            string EmployeeId,
            string PositionId,
            string DepartmentId,
            string BranchId,
            string? ManagerId,
            DateTime HireDate,
            DateTime? TerminationDate,
            string StatusId
        );

    public sealed record ContractDataResponse
        (
            string EmployeeId,
            string ContractId,
            string ContractNumber,
            string EmployeeTypeId,
            DateTime ContractStartDate,
            DateTime ContractEndDate,
            decimal Salary,
            string? ContractFileKey
        );
}
