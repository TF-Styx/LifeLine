using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Delete
{
    public sealed record DeleteGenderCommand(Guid Id) : IRequest<Result>;
}
