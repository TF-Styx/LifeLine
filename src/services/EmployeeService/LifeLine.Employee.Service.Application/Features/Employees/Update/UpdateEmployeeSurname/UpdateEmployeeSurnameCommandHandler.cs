using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeSurname
{
    public sealed class UpdateEmployeeSurnameCommandHandler
        (
            IWriteContext context, 
            IEmployeeRepository repository, 
            ILogger<UpdateEmployeeSurnameCommandHandler> logger
        ) : IRequestHandler<UpdateEmployeeSurnameCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<UpdateEmployeeSurnameCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateEmployeeSurnameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);

                if (entity == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись не найдена!"));

                entity.UpdateSurname(Surname.Create(request.Surname));

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
