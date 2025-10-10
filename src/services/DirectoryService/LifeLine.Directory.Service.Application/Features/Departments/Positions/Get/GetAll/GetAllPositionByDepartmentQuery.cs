using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAll
{
    public sealed record GetAllPositionByDepartmentQuery(Guid DepartmentId) : IRequest<List<PositionResponse>>;
}
