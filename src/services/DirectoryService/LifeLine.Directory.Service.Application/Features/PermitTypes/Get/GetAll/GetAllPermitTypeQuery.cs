using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.PermitTypes.Get.GetAll
{
    public sealed record GetAllPermitTypeQuery() : IRequest<List<PermitTypeResponse>>;
}
