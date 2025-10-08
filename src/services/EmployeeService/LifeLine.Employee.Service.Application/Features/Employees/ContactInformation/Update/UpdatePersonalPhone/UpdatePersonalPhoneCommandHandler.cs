using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdatePersonalPhone
{
    public sealed class UpdatePersonalPhoneCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository,
            ILogger<UpdatePersonalPhoneCommandHandler> logger
        ) : IRequestHandler<UpdatePersonalPhoneCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<UpdatePersonalPhoneCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdatePersonalPhoneCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.UpdatePersonalPhone(request.PersonalPhone);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении персонального номера телефона!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
