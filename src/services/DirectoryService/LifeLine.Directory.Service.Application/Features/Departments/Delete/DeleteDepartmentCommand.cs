using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Departments.Delete
{
    public sealed record DeleteDepartmentCommand(Guid DepartmentId) : IRequest<Result<Nothing>>;
}
