using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Get.GetAllByEmployeeId
{
    public sealed class GetAllAssignmentsContractsByEmployeeIdHandler(IWriteContext context) 
        : IRequestHandler<GetAllAssignmentsContractsByEmployeeIdQuery, List<AssignmentContractResponse>>
    {
        public async Task<List<AssignmentContractResponse>> Handle(GetAllAssignmentsContractsByEmployeeIdQuery request, CancellationToken cancellationToken)
            => await context.Assignments
               .Where(x => x.EmployeeId == request.EmployeeId)
               .Join
               (
                   context.Contracts,
                   assignment => assignment.ContractId,
                   contract => contract.Id,
                   (assignment, contract) => new { assignment, contract }
               )
               .Select
               (
                   x => new AssignmentContractResponse
                   (
                       new AssignmentResponse
                       (
                           x.assignment.Id.ToString(),
                           request.EmployeeId.ToString(),
                           x.assignment.PositionId.ToString(),
                           x.assignment.DepartmentId.ToString(),
                           x.assignment.BranchId.ToString(),
                           x.assignment.ManagerId.ToString(),
                           x.assignment.HireDate,
                           x.assignment.TerminationDate,
                           x.assignment.StatusId.ToString(),
                           x.assignment.ContractId.ToString()
                       ),
                       new ContractResponse
                       (
                           request.EmployeeId.ToString(),
                           x.contract!.Id.ToString(),
                           x.contract!.ContractNumber,
                           x.contract!.EmployeeTypeId.ToString(),
                           x.contract!.StartDate,
                           x.contract!.EndDate,
                           x.contract!.Salary,
                           x.contract!.FileKey
                       )
                   )
               ).ToListAsync(cancellationToken);
    }
}
