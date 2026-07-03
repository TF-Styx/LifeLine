using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Update
{
    public sealed record UpdateAssignmentCommand
    (
        Guid Id,
        Guid EmployeeId,
        Guid PositionId,
        Guid DepartmentId,
        Guid BranchId,
        Guid? ManagerId,
        DateTime HireDate,
        DateTime? TerminationDate,
        Guid StatusId,
        UpdateAssignmentContractCommand Contract
    ) : IRequest<Result<Nothing>>;

    public sealed record UpdateAssignmentContractCommand
    (
        Guid Id,
        Guid EmployeeId,
        Guid EmployeeTypeId,
        string ContractNumber,
        DateTime StartDate,
        DateTime EndDate,
        decimal Salary,
        string? BucketName,
        string? FileName
    );
}
