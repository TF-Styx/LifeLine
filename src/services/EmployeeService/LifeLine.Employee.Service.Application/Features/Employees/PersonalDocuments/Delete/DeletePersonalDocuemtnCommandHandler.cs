using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Delete
{
    public sealed class DeletePersonalDocuemtnCommandHandler
        (
            IWriteContext writeContext,
            IEmployeeRepository employeeRepository,
            ILogger<DeletePersonalDocuemtnCommandHandler> logger
        ) : IRequestHandler<DeletePersonalDocuemtnCommand, Result>
    {
        private readonly IWriteContext _writeContext = writeContext;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<DeletePersonalDocuemtnCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeletePersonalDocuemtnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

                if (employee is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.DeletePersonalDocument(request.PersonalDocumentId);

                await _writeContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении персонального документа!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
