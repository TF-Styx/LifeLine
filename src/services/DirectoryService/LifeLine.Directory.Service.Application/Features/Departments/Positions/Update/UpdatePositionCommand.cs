using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Update
{
    public sealed record UpdatePositionCommand(Guid DepartmentId, Guid PositionId, string Name, string Description) : IRequest<Result>;
}
