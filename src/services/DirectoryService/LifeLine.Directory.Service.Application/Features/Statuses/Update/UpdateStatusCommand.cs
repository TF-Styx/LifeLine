using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Update
{
    public sealed record UpdateStatusCommand(Guid Id, string Name, string Description) : IRequest<Result>;
}
