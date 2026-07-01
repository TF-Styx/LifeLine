using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Delete
{
    public sealed record DeleteHospitalCommand(Guid Id) : IRequest<Result>;
}
