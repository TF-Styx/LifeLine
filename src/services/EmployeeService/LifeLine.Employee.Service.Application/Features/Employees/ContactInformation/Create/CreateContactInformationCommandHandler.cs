using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Create
{
    public sealed class CreateContactInformationCommandHandler
        (
            IWriteContext context, 
            IEmployeeRepository repository
        ) : IRequestHandler<CreateContactInformationCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateContactInformationCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найдена!");

            employee.AddContactInformation
                (
                    request.PersonalPhone, 
                    request.CorporatePhone,
                    request.PersonalEmail,
                    request.CorporateEmail,
                    Address.Create
                        (
                            request.Address.PostalCode,
                            request.Address.Region,
                            request.Address.City,
                            request.Address.Street,
                            request.Address.Building,
                            request.Address.Apartment
                        )
                );

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
