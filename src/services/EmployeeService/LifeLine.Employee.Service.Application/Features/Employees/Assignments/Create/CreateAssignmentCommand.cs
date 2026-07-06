using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Create
{
    public sealed record CreateAssignmentCommand
    (
        Guid EmployeeId, 
        Guid PositionId, 
        Guid DepartmentId, 
        Guid? ManagerId, 
        DateTime HireDate, 
        DateTime? TerminationDate, 
        Guid StatusId, 
        CreateAssignmentContractCommand Contract
    ) : IRequest<Result<string>>;

    public sealed record CreateAssignmentContractCommand
    (
        Guid EmployeeTypeId, 
        string ContractNumber, 
        DateTime StartDate, 
        DateTime EndDate, 
        decimal Salary,
        string? BucketName,
        string? FileName
    );
}
