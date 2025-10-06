using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Delete
{
    public sealed record DeleteGenderCommand(Guid Id) : IRequest<Result>;
}
