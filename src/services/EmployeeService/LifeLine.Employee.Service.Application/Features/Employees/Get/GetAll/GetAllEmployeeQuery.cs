using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.Get.GetAll
{
    public sealed record GetAllEmployeeQuery : IRequest<List<EmployeeResponse>>;
}
