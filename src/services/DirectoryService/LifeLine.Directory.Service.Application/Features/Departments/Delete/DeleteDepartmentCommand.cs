using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Delete
{
    public sealed record DeleteDepartmentCommand(Guid DepartmentId) : IRequest<Result>;
}
