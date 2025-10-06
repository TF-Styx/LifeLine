using LifeLine.EmployeeService.Application.Abstraction.Models;
using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Genders.Get.GetAll
{
    public sealed record GetAllGendersQuery : IRequest<List<GenderResponse>>;
}
