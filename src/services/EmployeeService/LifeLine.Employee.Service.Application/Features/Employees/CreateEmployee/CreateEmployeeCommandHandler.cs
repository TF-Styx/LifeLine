using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Response.EmployeeService;
using Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.CreateEmployee
{
    public sealed class CreateEmployeeCommandHandler
        (
            IWriteContext writeContext,
            IEmployeeRepository employeeRepository,
            ILogger<CreateEmployeeCommandHandler> logger
        ) : IRequestHandler<CreateEmployeeCommand, Result<EmployeeIdResponse>>
    {
        private readonly IWriteContext _writeContext = writeContext;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<CreateEmployeeCommandHandler> _logger = logger;

        public async Task<Result<EmployeeIdResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = Domain.Models.Employee.Create(request.Surname, request.Name, request.Patronymic, request.GenderId);

                await _employeeRepository.AddAsync(employee, cancellationToken);
                await _writeContext.SaveChangesAsync(cancellationToken);

                return Result<EmployeeIdResponse>.Success(new EmployeeIdResponse(employee.Id));
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(Domain.Models.Employee.Create)}', в '{nameof(CreateEmployeeCommandHandler)}' при создании сотрудника не был сгенерирован {nameof(EmployeeId)}, в виде Guid!");
                    return Result<EmployeeIdResponse>.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result<EmployeeIdResponse>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Employee!");

                return Result<EmployeeIdResponse>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
