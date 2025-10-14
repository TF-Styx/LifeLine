using MediatR;
using Shared.Contracts.Request.Shared;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Assignments.Create
{
    public sealed record CreateAssignmentCommand(Guid EmployeeId, Guid PositionId, Guid DepartmentId, Guid? ManagerId, DateTime HireDate, DateTime TerminationDate, Guid StatusId, CreateAssignmentContractCommand Contract) : IRequest<Result>;

    public sealed record CreateAssignmentContractCommand(Guid EmployeeTypeId, string ContractNumber, DateTime StartDate, DateTime EndDate, decimal Salary, FileInput? FileKey);
}
