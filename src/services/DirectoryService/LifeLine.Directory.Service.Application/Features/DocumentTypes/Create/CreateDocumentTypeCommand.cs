using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.DocumentTypes.Create
{
    public sealed record CreateDocumentTypeCommand(string DocumentTypeName) : IRequest<Result>;
}
