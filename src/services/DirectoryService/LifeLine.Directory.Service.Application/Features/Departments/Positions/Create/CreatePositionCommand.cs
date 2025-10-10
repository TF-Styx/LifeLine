using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Create
{
    public sealed record CreatePositionCommand(Guid Id, string Name, string Description) : IRequest<Result>;
}
