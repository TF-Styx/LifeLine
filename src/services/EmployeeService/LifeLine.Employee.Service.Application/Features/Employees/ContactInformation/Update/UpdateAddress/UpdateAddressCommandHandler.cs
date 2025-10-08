using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateAddress
{
    public sealed class UpdateAddressCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository,
            ILogger<UpdateAddressCommandHandler> logger
        ) : IRequestHandler<UpdateAddressCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<UpdateAddressCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(request.EmployeeId);

                if (employee == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Пользователь не найден!"));

                employee.UpdateAddress(Address.Create(request.PostalCode, request.Region, request.City, request.Street, request.Building, request.Apartment));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении адреса проживания!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
