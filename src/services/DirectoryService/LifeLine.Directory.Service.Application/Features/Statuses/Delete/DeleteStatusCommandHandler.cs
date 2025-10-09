using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Delete
{
    public sealed class DeleteStatusCommandHandler
        (
            IDirectoryContext context,
            IStatusRepository repository,
            ILogger<DeleteStatusCommandHandler> logger
        ) : IRequestHandler<DeleteStatusCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IStatusRepository _repository = repository;
        private readonly ILogger<DeleteStatusCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var status = await _repository.GetByIdAsync(request.Id);

                if (status == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись статуса не найдена!"));

                _repository.Remove(status);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении Status!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
