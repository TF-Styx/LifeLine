using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Update
{
    public sealed record UpdateDepartmentCommand(Guid DepartmentId, string Name, string? Description, string Building, Guid BranchId) : IRequest<Result>;
}
