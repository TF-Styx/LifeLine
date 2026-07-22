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
        {
            var employee = await context.Employees.Include(x => x.Assignments).Include(x => x.Contracts)
                .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken: cancellationToken);

            if (employee == null)
                return null!;

            var items = employee!.Assignments.Select(
                a =>
                {
                    var contract = employee.Contracts.FirstOrDefault(c => c.Id == a.ContractId);

                    var assignments = new AssignmentResponse
                    (
                        a.Id.ToString(),
                        request.EmployeeId.ToString(),
                        a.PositionId.ToString(),
                        a.DepartmentId.ToString(),
                        a.BranchId.ToString(),
                        a.ManagerId.ToString(),
                        a.HireDate,
                        a.TerminationDate,
                        a.StatusId.ToString(),
                        contract!.Id.ToString()
                    );

                    var contracts = new ContractResponse
                    (
                        request.EmployeeId.ToString(),
                        contract!.Id.ToString(),
                        contract!.ContractNumber,
                        contract!.EmployeeTypeId.ToString(),
                        contract!.StartDate,
                        contract!.EndDate,
                        contract!.Salary,
                        contract!.FileKey
                    );

                    return new AssignmentContractResponse(assignments, contracts);
                }).ToList();

            return items;
        }
    }
}
