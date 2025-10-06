using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Update
{
    public sealed class UpdateGenderNameCommandHandler
        (
            IWriteContext context, 
            IGenderRepository repository, 
            ILogger<UpdateGenderNameCommandHandler> logger
        ) : IRequestHandler<UpdateGenderNameCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IGenderRepository _repository = repository;
        private readonly ILogger<UpdateGenderNameCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateGenderNameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);

                if (entity == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена!"));

                entity.UpdateName(GenderName.Create(request.Name));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении Gender!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
