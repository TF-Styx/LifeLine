using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Specialties.Create
{
    public sealed record CreateSpecialtyCommand(string SpecialtyName, string? Description) : IRequest<Result>;
}
