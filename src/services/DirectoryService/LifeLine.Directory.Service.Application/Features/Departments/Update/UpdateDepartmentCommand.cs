using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Update
{
    public sealed record UpdateDepartmentCommand(Guid Id, string Name, string Description) : IRequest<Result>;
}
