using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.ForceDelete
{
    public sealed record ForceDeleteDepartmentCommand(Guid Id) : IRequest<Result>;
}
