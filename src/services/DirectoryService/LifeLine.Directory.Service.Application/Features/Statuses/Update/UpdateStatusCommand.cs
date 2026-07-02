using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Update
{
    public sealed record UpdateStatusCommand(Guid Id, string Name, string Description) : IRequest<Result<Nothing>;
}
