using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.PermitTypes.Create
{
    public sealed record CreatePermitTypeCommand(string PermitTypeName) : IRequest<Result>;
}
