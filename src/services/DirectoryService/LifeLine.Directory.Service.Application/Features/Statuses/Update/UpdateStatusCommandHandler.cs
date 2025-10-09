using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Update
{
    public sealed class UpdateStatusCommandHandler
        (
            IDirectoryContext context,
            IStatusRepository repository,
            ILogger<UpdateStatusCommandHandler> logger
        ) : IRequestHandler<UpdateStatusCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IStatusRepository _repository = repository;
        private readonly ILogger<UpdateStatusCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var status = await _repository.GetByIdAsync(request.Id);

                if (status == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись статуса не найдена!"));

                status.UpdateStatusName(DirectoryName.Create(request.Name));
                status.UpdateStatusDescription(Description.Create(request.Description));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении Status!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
