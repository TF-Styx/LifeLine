using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdatePersonalEmail
{
    public sealed class UpdatePersonalEmailCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository,
            ILogger<UpdatePersonalEmailCommandHandler> logger
        ) : IRequestHandler<UpdatePersonalEmailCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<UpdatePersonalEmailCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdatePersonalEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден"));

                employee.UpdatePersonalEmail(request.PersonalEmail);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении персональной почты!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
