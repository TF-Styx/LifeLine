using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Get.GetAll
{
    public sealed record GetAllStatusQuery : IRequest<List<StatusResponse>>;
}
