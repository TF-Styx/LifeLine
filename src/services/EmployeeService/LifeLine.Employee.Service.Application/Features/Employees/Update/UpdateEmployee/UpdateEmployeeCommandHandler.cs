using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository,
            ILogger<UpdateEmployeeCommandHandler> logger
        ) : IRequestHandler<UpdateEmployeeCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<UpdateEmployeeCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);

                if (entity == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена!"));

                entity.UpdateSurname(Surname.Create(request.Surname));
                entity.UpdateName(Name.Create(request.Name));
                entity.UpdatePatronymic(Patronymic.Create(request.Patronymic));
                entity.UpdateGenderId(GenderId.Create(request.GenderId));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении Employee!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
