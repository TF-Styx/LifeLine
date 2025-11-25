using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.Get.GetAllForHr
{
    public sealed record GetAllEmployeeForHrQuery() : IRequest<List<EmployeeHrItemResponse>>;
}
