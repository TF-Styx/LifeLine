using MediatR;
using Terminex.Common.Results;
using Shared.Api.Application.Validators.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Genders.Create
{
    public sealed record CreateGenderCommand(string Name) : IRequest<Result<string>>, IHasName;
}
