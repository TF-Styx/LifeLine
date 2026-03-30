using MediatR;
using Shared.Contracts.Request.Shared;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.CreateMany
{
    public sealed record CreateManyAssignmentsCommand(Guid EmployeeId, List<CreateManyDataAssignmentsCommand> Assignments) : IRequest<Result>;

    public sealed record CreateManyDataAssignmentsCommand(Guid PositionId, Guid DepartmentId, Guid? ManagerId, DateTime HireDate, DateTime? TerminationDate, Guid StatusId, CreateManyDataAssignmentContractCommand Contracts);

    public sealed record CreateManyDataAssignmentContractCommand(Guid EmployeeTypeId, string ContractNumber, DateTime StartDate, DateTime EndDate, decimal Salary, FileInput? FileKey);
}
