using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Branches.Delete
{
    public sealed record DeleteBranchCommand(Guid Id) : IRequest<Result<Nothing>>;
}
