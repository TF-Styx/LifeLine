namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record AssignmentResponse
        (
            Guid AssignmentId,
            Guid EmployeeId,
            Guid PositionId,
            Guid DepartmentId,
            Guid? ManagerId,
            DateTime HireDate,
            DateTime? TerminationDate,
            Guid StatusId
        );
}
