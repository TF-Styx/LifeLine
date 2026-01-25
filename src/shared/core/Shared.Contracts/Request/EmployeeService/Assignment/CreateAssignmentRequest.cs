namespace Shared.Contracts.Request.EmployeeService.Assignment
{
    public sealed record CreateAssignmentRequest(Guid PositionId, Guid DepartmentId, Guid? ManagerId, DateTime HireDate, DateTime? TerminationDate, Guid StatusId, CreateAssignmentContractRequest Contract);

    public sealed record CreateAssignmentContractRequest(Guid EmployeeTypeId, string ContractNumber, DateTime StartDate, DateTime EndDate, decimal Salary/*, IFormFile? FileKey*/);
}
