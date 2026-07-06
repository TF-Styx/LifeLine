using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Get.GetAllWorkPermitByEmployeeId
{
    public sealed record GetAllWorkPermitByEmployeeIdQuery(Guid EmployeeId) : IRequest<List<WorkPermitResponse>>;
}
