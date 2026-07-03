using MediatR;
using Terminex.Common.Results;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.DocumentTypes.Create
{
    public sealed class CreateDocumentTypeCommandHandler(IDirectoryContext context) : IRequestHandler<CreateDocumentTypeCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            var documentType = DocumentType.Create(request.DocumentTypeName);

            await context.DocumentTypes.AddAsync(documentType, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return documentType.Id.ToString();
        }
    }
}
