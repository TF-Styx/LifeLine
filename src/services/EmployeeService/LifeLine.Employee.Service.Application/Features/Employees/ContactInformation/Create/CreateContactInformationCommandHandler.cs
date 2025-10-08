using LifeLine.Employee.Service.Domain.Exceptions;
using LifeLine.Employee.Service.Domain.ValueObjects.ContactInformation;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Create
{
    public sealed class CreateContactInformationCommandHandler
        (
            IWriteContext context, 
            IEmployeeRepository repository, 
            ILogger<CreateContactInformationCommandHandler> logger
        ) : IRequestHandler<CreateContactInformationCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<CreateContactInformationCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateContactInformationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найдена!"));

                employee.AddContactInformation
                    (
                        request.PersonalPhone, 
                        request.CorporatePhone,
                        request.PersonalEmail,
                        request.CorporateEmail,
                        Address.Create
                            (
                                request.Address.PostalCode,
                                request.Address.Region,
                                request.Address.City,
                                request.Address.Street,
                                request.Address.Building,
                                request.Address.Apartment
                            )
                    );

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is ExistContactInformationException existContInfoEX)
                    return Result.Failure(new Error(ErrorCode.ExistContactInformation, existContInfoEX.Message));

                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(Domain.Models.ContactInformation.Create)}', в '{nameof(CreateContactInformationCommandHandler)}' при создании контактной информации сотрудника не был сгенерирован {nameof(ContactInformationId)}, в виде Guid!");
                    return Result.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании контактной информации!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
