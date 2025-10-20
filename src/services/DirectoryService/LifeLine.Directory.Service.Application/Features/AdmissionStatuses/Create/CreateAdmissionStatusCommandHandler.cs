using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Create
{
    public sealed class CreateAdmissionStatusCommandHandler
        (
            IDirectoryContext context,
            ILogger<CreateAdmissionStatusCommandHandler> logger
        ) : IRequestHandler<CreateAdmissionStatusCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly ILogger<CreateAdmissionStatusCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateAdmissionStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var admissionStatus = AdmissionStatus.Create(request.AdmissionName);

                await _context.AdmissionStatuses.AddAsync(admissionStatus, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(AdmissionStatus.Create)}', в '{nameof(CreateAdmissionStatusCommandHandler)}' при создании статуса сертификата не был сгенерирован {nameof(StatusId)}, в виде Guid!");
                    return Result<Guid>.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result<Guid>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании AdmissionStatus!");

                return Result<Guid>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
