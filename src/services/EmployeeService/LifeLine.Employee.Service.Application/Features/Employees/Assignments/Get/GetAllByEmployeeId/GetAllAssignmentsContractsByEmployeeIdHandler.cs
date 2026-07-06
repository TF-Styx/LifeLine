using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Get.GetAllByEmployeeId
{
    public sealed class GetAllAssignmentsContractsByEmployeeIdHandler(IWriteContext context) 
        : IRequestHandler<GetAllAssignmentsContractsByEmployeeIdQuery, AssignmentContractResponse>
    {
        public async Task<AssignmentContractResponse> Handle(GetAllAssignmentsContractsByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await context.Employees.Include(x => x.Assignments).Include(x => x.Contracts)
                .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken: cancellationToken);

            var assignments = employee!.Assignments.Select
            (
                a => new AssignmentDataResponse
                (
                    a.Id.ToString(),
                    request.EmployeeId.ToString(),
                    a.PositionId.ToString(),
                    a.DepartmentId.ToString(),
                    a.BranchId.ToString(),
                    a.ManagerId.ToString(),
                    a.HireDate,
                    a.TerminationDate,
                    a.StatusId.ToString()
                )
            ).ToList();

            var contracts = employee.Contracts.Select
            (
                c => new ContractDataResponse
                (
                    request.EmployeeId.ToString(),
                    c.Id.ToString(),
                    c.ContractNumber,
                    c.EmployeeTypeId.ToString(),
                    c.StartDate,
                    c.EndDate,
                    c.Salary,
                    c.FileKey
                )
            ).ToList();

            return new AssignmentContractResponse(new AssignmentContractDataResponse(assignments, contracts));
        }
    }
}
