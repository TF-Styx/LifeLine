using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAllPosition
{
    public sealed record GetAllPositionQuery() : IRequest<List<PositionResponse>>;
}
