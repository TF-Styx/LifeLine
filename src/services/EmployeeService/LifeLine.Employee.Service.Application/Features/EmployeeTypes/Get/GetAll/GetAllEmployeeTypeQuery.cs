using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Get.GetAll
{
    public sealed record GetAllEmployeeTypeQuery : IRequest<List<EmployeeTypeResponse>>;
}
