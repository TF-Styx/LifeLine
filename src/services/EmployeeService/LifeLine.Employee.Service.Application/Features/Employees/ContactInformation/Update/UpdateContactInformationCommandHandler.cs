using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update
{
    public sealed class UpdateContactInformationCommandHandler
        (
            IWriteContext writeContext,
            IEmployeeRepository employeeRepository,
            ILogger<UpdateContactInformationCommandHandler> logger
        ) : IRequestHandler<UpdateContactInformationCommand, Result>
    {
        private readonly IWriteContext _writeContext = writeContext;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<UpdateContactInformationCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateContactInformationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(Guid.Parse(request.EmployeeId));

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.UpdatePersonalPhone(request.PersonalPhone);
                employee.UpdateCorporatePhone(request.CorporatePhone);
                employee.UpdatePersonalEmail(request.PersonalEmail);
                employee.UpdateCorporateEmail(request.CorporateEmail);
                employee.UpdateAddress(Address.Create(request.PostalCode, request.Region, request.City, request.Street, request.Building, request.Apartment));

                await _writeContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении контактной информации!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
