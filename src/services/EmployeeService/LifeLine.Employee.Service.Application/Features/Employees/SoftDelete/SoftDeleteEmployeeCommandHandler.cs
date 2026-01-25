using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.SoftDelete
{
    public sealed class SoftDeleteEmployeeCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository employeeRepository,
            ILogger<SoftDeleteEmployeeCommandHandler> logger
        ) : IRequestHandler<SoftDeleteEmployeeCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<SoftDeleteEmployeeCommandHandler> _logger = logger;

        public async Task<Result> Handle(SoftDeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.Deactivate();

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при деактивации пользователя!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
