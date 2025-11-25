using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Specialties.Get.GetAll
{
    public sealed record GetAllSpecialtyQuery() : IRequest<List<SpecialtyResponse>>;
}
