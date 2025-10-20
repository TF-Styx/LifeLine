using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.DocumentTypes.Create
{
    public sealed record CreateDocumentTypeCommand(string DocumentTypeName) : IRequest<Result>;
}
