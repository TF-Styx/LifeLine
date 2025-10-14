using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Delete
{
    public sealed class DeleteEmployeeTypeCommandHandler
        (
            IWriteContext context,
            IEmployeeTypeRepository employeeTypeRepository,
            ILogger<DeleteEmployeeTypeCommandHandler> logger
        ) : IRequestHandler<DeleteEmployeeTypeCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeTypeRepository _employeeTypeRepository = employeeTypeRepository;
        private readonly ILogger<DeleteEmployeeTypeCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeleteEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employeeType = await _employeeTypeRepository.GetByIdAsync(request.Id);

                if (employeeType == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись типа занятости не найдена!"));

                _employeeTypeRepository.Remove(employeeType);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении типа занятости!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
