using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Delete
{
    public sealed record DeleteStatusCommand(Guid Id) : IRequest<Result>;
}
