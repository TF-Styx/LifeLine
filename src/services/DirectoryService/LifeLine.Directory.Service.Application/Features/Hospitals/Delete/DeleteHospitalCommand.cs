using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Delete
{
    public sealed record DeleteHospitalCommand(Guid Id) : IRequest<Result<Nothing>>;
}
