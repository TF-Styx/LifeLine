using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Create
{
    public sealed record CreateGenderCommand(string Name) : IRequest<Result>, IHasName;
}
