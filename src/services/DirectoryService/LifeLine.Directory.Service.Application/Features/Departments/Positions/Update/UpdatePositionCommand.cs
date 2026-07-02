using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Update
{
    public sealed record UpdatePositionCommand(Guid DepartmentId, Guid PositionId, string Name, string? Description) : IRequest<Result<Nothing>>;
}
