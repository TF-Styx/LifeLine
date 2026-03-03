using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Update
{
    public sealed record UpdateGenderNameCommand(Guid Id, string Name) : IRequest<Result>, IHasName;
}
