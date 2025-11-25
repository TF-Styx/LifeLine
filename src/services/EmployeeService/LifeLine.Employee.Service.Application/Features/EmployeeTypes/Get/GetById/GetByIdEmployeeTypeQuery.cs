using MediatR;
using Shared.Contracts.Response.EmployeeService;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Get.GetById
{
    public sealed record GetByIdEmployeeTypeQuery(Guid Id) : IRequest<Result<EmployeeTypeResponse>>;
}
