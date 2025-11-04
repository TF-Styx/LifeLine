using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Get.GetAll
{
    public sealed record GetAllAdmissionStatusQuery() : IRequest<List<AdmissionStatusResponse>>;
}
