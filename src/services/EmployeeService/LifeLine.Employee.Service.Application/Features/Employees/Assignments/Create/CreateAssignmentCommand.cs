using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using Shared.Contracts.Request.Shared;

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
    ) : IRequest<Result<Nothing>>;

    public sealed record CreateAssignmentContractCommand
    (
        Guid EmployeeTypeId, 
        string ContractNumber, 
        DateTime StartDate, 
        DateTime EndDate, 
        decimal Salary, 
        FileInput? FileKey
    );
}
