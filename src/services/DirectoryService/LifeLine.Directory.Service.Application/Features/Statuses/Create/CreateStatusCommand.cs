using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Create
{
    public sealed record CreateStatusCommand(string Name, string Description) : IRequest<Result>;
}
