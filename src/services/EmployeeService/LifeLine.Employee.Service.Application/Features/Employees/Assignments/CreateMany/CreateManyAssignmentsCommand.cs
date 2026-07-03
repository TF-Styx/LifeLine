using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.CreateMany
{
    public sealed record CreateManyAssignmentsCommand(Guid EmployeeId, List<CreateManyDataAssignmentsCommand> Assignments) : IRequest<Result<Nothing>>;

    public sealed record CreateManyDataAssignmentsCommand
    (
        Guid PositionId, 
        Guid DepartmentId, 
        Guid BranchId, 
        Guid? ManagerId, 
        DateTime HireDate, 
        DateTime? TerminationDate, 
        Guid StatusId, 
        CreateManyDataAssignmentContractCommand Contracts
    );

    public sealed record CreateManyDataAssignmentContractCommand
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
