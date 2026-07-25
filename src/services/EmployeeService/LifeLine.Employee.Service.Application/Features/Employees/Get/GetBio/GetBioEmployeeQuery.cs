using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.Get.GetBio
{
    public sealed record GetBioEmployeeQuery(Guid EmployeeId) : IRequest<EmployeeBioResponse?>;
}
