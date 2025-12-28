using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Delete
{
    public sealed class DeleteAssignmentCommandHandler
        (
            IWriteContext writeContext,
            IEmployeeRepository employeeRepository,
            ILogger<DeleteAssignmentCommandHandler> logger
        ) : IRequestHandler<DeleteAssignmentCommand, Result>
    {
        private readonly IWriteContext _writeContext = writeContext;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<DeleteAssignmentCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.DeleteAssignmentContract(request.AssignmentId);

                await _writeContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении назначения и контракта!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
