using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Delete
{
    public sealed record DeleteStatusCommand(Guid Id) : IRequest<Result<Nothing>>;
}
