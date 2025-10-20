using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.PermitTypes.Create
{
    public sealed class CreatePermitTypeCommandHandler
        (
            IDirectoryContext context,
            ILogger<CreatePermitTypeCommandHandler> logger
        ) : IRequestHandler<CreatePermitTypeCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly ILogger<CreatePermitTypeCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreatePermitTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var permitType = PermitType.Create(request.PermitTypeName);

                await _context.PermitTypes.AddAsync(permitType, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(PermitType.Create)}', в '{nameof(CreatePermitTypeCommandHandler)}' при создании типа документа (сертификат или аккредитация) не был сгенерирован {nameof(PermitTypeId)}, в виде Guid!");
                    return Result<Guid>.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result<Guid>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании PermitType!");

                return Result<Guid>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
