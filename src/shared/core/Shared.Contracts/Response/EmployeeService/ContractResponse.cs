namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record ContractResponse
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
