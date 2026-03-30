using LifeLine.Employee.Service.Domain.Exceptions;
using LifeLine.Employee.Service.Domain.ValueObjects.PersonalDocuments;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create.CreateMany
{
    public sealed class CreateManyEmployeeSpecialtiesCommandHandler
        (
            IWriteContext writeContext,
            IEmployeeRepository employeeRepository,
            ILogger<CreateManyEmployeeSpecialtiesCommandHandler> logger
        ) : IRequestHandler<CreateManyEmployeeSpecialtiesCommand, Result>
    {
        private readonly IWriteContext _writeContext = writeContext;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly ILogger<CreateManyEmployeeSpecialtiesCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateManyEmployeeSpecialtiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(Error.NotFound("Пользователь не найден!"));

                foreach (var item in request.Specialties)
                    employee.AddSpecialty(item.SpecialtyId);

                await _writeContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is ExistEmployeeSpecialtyExeption existEmpSpecEX)
                    return Result.Failure(new Error(AppErrors.ExistEmployeeSPecialty, existEmpSpecEX.Message));

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при добавлении специальностей!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
