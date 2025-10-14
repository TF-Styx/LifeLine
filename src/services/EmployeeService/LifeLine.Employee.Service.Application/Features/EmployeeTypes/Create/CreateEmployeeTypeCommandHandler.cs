using LifeLine.Employee.Service.Application.Features.Employees.Create;
using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.EmployeeType;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Create
{
    public sealed class CreateEmployeeTypeCommandHandler
        (
            IWriteContext context,
            IEmployeeTypeRepository employeeTypeRepository,
            ILogger<CreateEmployeeTypeCommandHandler> logger
        ) : IRequestHandler<CreateEmployeeTypeCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeTypeRepository _employeeTypeRepository = employeeTypeRepository;
        private readonly ILogger<CreateEmployeeTypeCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employeeType = EmployeeType.Create(request.Name, request.Description);

                await _employeeTypeRepository.AddAsync(employeeType, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(EmployeeType.Create)}', в '{nameof(CreateEmployeeTypeCommandHandler)}' при создании сотрудника не был сгенерирован {nameof(EmployeeTypeId)}, в виде Guid!");
                    return Result.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании типа занятости!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
