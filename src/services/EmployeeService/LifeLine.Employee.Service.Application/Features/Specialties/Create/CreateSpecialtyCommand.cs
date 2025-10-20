using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Specialties.Create
{
    public sealed record CreateSpecialtyCommand(string SpecialtyName, string? Description) : IRequest<Result>;
}
