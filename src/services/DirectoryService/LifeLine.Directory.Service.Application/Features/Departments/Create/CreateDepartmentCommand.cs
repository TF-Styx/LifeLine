using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Create
{
    public sealed record CreateDepartmentCommand(string Name, string? Description, string Building, Guid BranchId) : IRequest<Result<string>>;
}
