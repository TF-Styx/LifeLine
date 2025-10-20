using LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Add;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create
{
    public sealed class CreateEmployeeSpecialtyCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository employeeRepository,
            ILogger<CreateEmployeeSpecialtyCommandHandler> logger
        ) : IRequestHandler<CreateEmployeeSpecialtyCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<CreateEmployeeSpecialtyCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateEmployeeSpecialtyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.AddSpecialty(request.SpecialtyId);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при добавлении специальностей сотрудника!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
