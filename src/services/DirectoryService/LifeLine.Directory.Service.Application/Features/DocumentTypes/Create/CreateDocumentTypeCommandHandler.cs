using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.DocumentTypes.Create
{
    public sealed class CreateDocumentTypeCommandHandler
        (
            IDirectoryContext context,
            ILogger<CreateDocumentTypeCommandHandler> logger
        ) : IRequestHandler<CreateDocumentTypeCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly ILogger<CreateDocumentTypeCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateDocumentTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var documentType = DocumentType.Create(request.DocumentTypeName);

                await _context.DocumentTypes.AddAsync(documentType, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(DocumentType.Create)}', в '{nameof(CreateDocumentTypeCommandHandler)}' при создании типа документа не был сгенерирован {nameof(DocumentTypeId)}, в виде Guid!");
                    return Result<Guid>.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result<Guid>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании DocumentType!");

                return Result<Guid>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
