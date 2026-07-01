using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Get.GetAllByBranchId
{
    public sealed record GetAllByBranchIdQuery(Guid BranchId) : IRequest<List<DepartmentResponse>>;
}
