using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.EducationLevels.Get.GetAll
{
    public sealed record GetAllEducationLevelQuery : IRequest<List<EducationLevelResponse>>;
}
