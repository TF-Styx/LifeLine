using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.Get.GetFullDetailsForEmployee
{
    public sealed record GetFullDetailsForEmployeeQuery(Guid EmployeeId) : IRequest<EmployeeFullDetailsResponse?>;
}
