using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Delete
{
    public sealed class DeleteGenderCommandHandler
        (
            IWriteContext context,
            IGenderRepository repository,
            ILogger<DeleteGenderCommandHandler> logger
        ) : IRequestHandler<DeleteGenderCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IGenderRepository _repository = repository;
        private readonly ILogger<DeleteGenderCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeleteGenderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);

                if (entity == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена!"));

                _repository.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении Gender!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
