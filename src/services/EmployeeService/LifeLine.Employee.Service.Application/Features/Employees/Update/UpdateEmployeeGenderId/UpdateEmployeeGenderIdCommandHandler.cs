using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeGenderId
{
    public sealed class UpdateEmployeeGenderIdCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository employeeRepository,
            IGenderRepository genderRepository,
            ILogger<UpdateEmployeeGenderIdCommandHandler> logger
        ) : IRequestHandler<UpdateEmployeeGenderIdCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IGenderRepository _genderRepository = genderRepository;
        private readonly ILogger<UpdateEmployeeGenderIdCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateEmployeeGenderIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _employeeRepository.GetByIdAsync(request.Id);
                var existGenderId = await _genderRepository.ExistByIdAsync(request.GenderId);

                if (entity == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данный сотрудник не найден!"));

                if (!existGenderId)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данный гендер не найден!"));

                entity.UpdateGenderId(GenderId.Create(request.GenderId));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();

            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении Employee!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
