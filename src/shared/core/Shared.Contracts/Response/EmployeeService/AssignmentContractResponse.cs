namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record AssignmentContractResponse(AssignmentResponse Assignment, ContractResponse Contract);

    //public sealed record AssignmentResponse
    //    (
    //        string AssignmentId,
    //        string EmployeeId,
    //        string PositionId,
    //        string DepartmentId,
    //        string BranchId,
    //        string? ManagerId,
    //        DateTime HireDate,
    //        DateTime? TerminationDate,
    //        string StatusId,
    //        string ContractId
    //    );

    //public sealed record ContractResponse
    //    (
    //        string EmployeeId,
    //        string ContractId,
    //        string ContractNumber,
    //        string EmployeeTypeId,
    //        DateTime ContractStartDate,
    //        DateTime ContractEndDate,
    //        decimal Salary,
    //        string? ContractFileKey
    //    );
}
