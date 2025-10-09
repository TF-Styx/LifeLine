using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Delete
{
    public sealed record DeleteDepartmentCommand(Guid Id) : IRequest<Result>;
}
