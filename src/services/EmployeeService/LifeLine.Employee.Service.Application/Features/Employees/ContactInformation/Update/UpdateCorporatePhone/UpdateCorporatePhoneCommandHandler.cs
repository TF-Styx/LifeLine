using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateCorporatePhone
{
    public sealed class UpdateCorporatePhoneCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository,
            ILogger<UpdateCorporatePhoneCommandHandler> logger
        ) : IRequestHandler<UpdateCorporatePhoneCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<UpdateCorporatePhoneCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateCorporatePhoneCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.UpdateCorporatePhone(request.CorporatePhone);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении корпоративной почты!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
