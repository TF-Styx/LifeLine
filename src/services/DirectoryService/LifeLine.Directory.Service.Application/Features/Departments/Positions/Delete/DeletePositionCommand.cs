using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Delete
{
    public sealed record DeletePositionCommand(Guid DepartmentId, Guid PositionId) : IRequest<Result<Nothing>>;
}
