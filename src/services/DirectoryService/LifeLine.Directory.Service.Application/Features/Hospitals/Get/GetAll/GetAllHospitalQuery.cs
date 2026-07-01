using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Get.GetAll
{
    public sealed record GetAllHospitalQuery : IRequest<List<HospitalResponse>>;
}
