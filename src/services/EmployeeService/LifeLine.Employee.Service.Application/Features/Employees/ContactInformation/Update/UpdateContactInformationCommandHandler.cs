using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Shared.Domain.ValueObjects;
using Terminex.Common.Primitives;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update
{
    public sealed class UpdateContactInformationCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<UpdateContactInformationCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateContactInformationCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(Guid.Parse(request.EmployeeId));

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.UpdatePersonalPhone(request.PersonalPhone);

            if (!string.IsNullOrWhiteSpace(request.CorporatePhone))
                employee.UpdateCorporatePhone(request.CorporatePhone);

            employee.UpdatePersonalEmail(request.PersonalEmail);

            if (!string.IsNullOrWhiteSpace(request.CorporateEmail))
                employee.UpdateCorporateEmail(request.CorporateEmail);

            employee.UpdateAddress(Address.Create(request.PostalCode, request.Region, request.City, request.Street, request.Building, request.Apartment));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
