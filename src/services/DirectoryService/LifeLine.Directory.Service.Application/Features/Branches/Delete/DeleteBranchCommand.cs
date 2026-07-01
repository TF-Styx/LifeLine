using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Branches.Delete
{
    public sealed record DeleteBranchCommand(Guid DepartmentId) : IRequest<Result>;
}
