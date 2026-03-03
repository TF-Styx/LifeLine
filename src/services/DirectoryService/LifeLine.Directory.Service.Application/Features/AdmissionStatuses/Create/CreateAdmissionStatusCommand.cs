using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Create
{
    public sealed record CreateAdmissionStatusCommand(string AdmissionName) : IRequest<Result>;
}
