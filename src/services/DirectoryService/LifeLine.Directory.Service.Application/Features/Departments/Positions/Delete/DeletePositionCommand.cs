using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Delete
{
    public sealed record DeletePositionCommand(Guid DepartmentId, Guid PositionId) : IRequest<Result>;
}
