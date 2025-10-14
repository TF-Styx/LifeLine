using LifeLine.Employee.Service.Domain.ValueObjects.EmployeeType;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Update
{
    public sealed class UpdateEmployeeTypeCommandHandler
        (
            IWriteContext context,
            IEmployeeTypeRepository employeeTypeRepository,
            ILogger<UpdateEmployeeTypeCommandHandler> logger
        ) : IRequestHandler<UpdateEmployeeTypeCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeTypeRepository _employeeTypeRepository = employeeTypeRepository;
        private readonly ILogger<UpdateEmployeeTypeCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employeeType = await _employeeTypeRepository.GetByIdAsync(request.Id);

                if (employeeType == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись типа занятости не найдена!"));

                employeeType.UpdateName(EmployeeTypeName.Create(request.Name));
                employeeType.UpdateDescription(Description.Create(request.Description));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении тпа занятости!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
