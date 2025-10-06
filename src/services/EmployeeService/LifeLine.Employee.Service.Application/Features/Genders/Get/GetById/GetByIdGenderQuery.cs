using MediatR;
using Shared.Contracts.Response.EmployeeService;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Get.GetById
{
    public sealed record GetByIdGenderQuery(Guid Id) : IRequest<Result<GenderResponse>>;
}
