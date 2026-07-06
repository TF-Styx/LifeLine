using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Get.GetAllByEmployeeId
{
    public sealed record GetAllAssignmentsContractsByEmployeeIdQuery(Guid EmployeeId) : IRequest<AssignmentContractResponse>;
}
