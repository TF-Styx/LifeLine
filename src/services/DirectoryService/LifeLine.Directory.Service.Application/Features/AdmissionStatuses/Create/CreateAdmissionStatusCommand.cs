using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Create
{
    public sealed record CreateAdmissionStatusCommand(string AdmissionName) : IRequest<Result>;
}
