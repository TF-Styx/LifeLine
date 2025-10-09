using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Get.GetAll
{
    public sealed record GetAllDepartmentQuery : IRequest<List<DepartmentResponse>>;
}
