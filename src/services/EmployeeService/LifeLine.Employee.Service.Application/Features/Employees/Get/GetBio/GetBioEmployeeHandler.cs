using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.Get.GetBio
{
    public sealed class GetBioEmployeeHandler(IWriteContext context) : IRequestHandler<GetBioEmployeeQuery, EmployeeBioResponse?>
    {
        public async Task<EmployeeBioResponse?> Handle(GetBioEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employee = await context.Employees
                .AsNoTracking()
                .Include(x => x.Gender)
                .Include(x => x.ContactInformation)
                .Include(x => x.EmployeeSpecialties)
                .FirstOrDefaultAsync(x => x.Id == request.EmployeeId);

            if (employee == null)
                return null;

            ContactInformationResponse? contactResponse = null;
            if (employee.ContactInformation != null)
                contactResponse = new ContactInformationResponse
                (
                    employee.ContactInformation.Id.ToString(),
                    employee.ContactInformation.PersonalPhone,
                    employee.ContactInformation.CorporatePhone ?? "",
                    employee.ContactInformation.PersonalEmail,
                    employee.ContactInformation.CorporateEmail ?? "",
                    employee.ContactInformation.HomeAddress.PostalCode,
                    employee.ContactInformation.HomeAddress.Region,
                    employee.ContactInformation.HomeAddress.City,
                    employee.ContactInformation.HomeAddress.Street,
                    employee.ContactInformation.HomeAddress.Building ?? "",
                    employee.ContactInformation.HomeAddress.Apartment ?? ""
                );

            var response = new EmployeeBioResponse(
                employee.Id.ToString(),
                employee.Surname,
                employee.Name,
                employee.Patronymic ?? "",
                employee.Gender.Id.ToString(),
                employee.PersonalPhoto,
                contactResponse,
                employee.EmployeeSpecialties?.Select(x => x.SpecialtyId.ToString()).ToList() ?? []
            );

            return response;
        }
    }
}
