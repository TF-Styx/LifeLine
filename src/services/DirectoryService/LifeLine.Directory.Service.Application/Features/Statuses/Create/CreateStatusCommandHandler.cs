using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Create
{
    public sealed class CreateStatusCommandHandler
        (
            IDirectoryContext context, 
            IStatusRepository repository,
            ILogger<CreateStatusCommandHandler> logger
        ) : IRequestHandler<CreateStatusCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IStatusRepository _repository = repository;
        private readonly ILogger<CreateStatusCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var status = Status.Create(request.Name, request.Description);

                await _repository.AddAsync(status, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(Status.Create)}', в '{nameof(CreateStatusCommandHandler)}' при создании сотрудника не был сгенерирован {nameof(StatusId)}, в виде Guid!");
                    return Result.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Status!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
