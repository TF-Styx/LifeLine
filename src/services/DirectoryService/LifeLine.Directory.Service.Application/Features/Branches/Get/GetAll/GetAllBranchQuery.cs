using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Branches.Get.GetAll
{
    public sealed record GetAllBranchQuery : IRequest<List<BranchResponse>>;
}
