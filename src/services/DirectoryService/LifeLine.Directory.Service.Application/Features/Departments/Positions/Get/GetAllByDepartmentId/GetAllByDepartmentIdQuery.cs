using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAllByDepartmentId
{
    public sealed record GetAllByDepartmentIdQuery(Guid DepartmentId) : IRequest<List<PositionResponse>>;
}
