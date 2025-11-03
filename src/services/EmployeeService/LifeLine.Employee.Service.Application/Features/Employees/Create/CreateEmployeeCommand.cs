using MediatR;
using Shared.Api.Application.Validators.Abstraction;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Create
{
    public sealed record CreateEmployeeCommand
        (
            string Surname, string Name, string? Patronymic, Guid GenderId,
            List<CreatePersonalDocumentCommand>? PersonalDocuments, 
            CreateContactInformationCommand? CreateContactInfo,
            List<CreateEducationDocumentCommand>? CreateEducationDoc
        ) : IRequest<Result>, IHasFIO;

    public sealed record CreatePersonalDocumentCommand(Guid DocumentTypeId, string Number, string? Series);

    public sealed record CreateContactInformationCommand(string PersonalPhone, string? CorporatePhone, string PersonalEmail, string? CorporateEmail, CreateAddressCommandData? Address);

    public sealed record CreateAddressCommandData(string PostalCode, string Region, string City, string Street, string Building, string? Apartment);

    public sealed record CreateEducationDocumentCommand(Guid EducationLevelId, Guid DocumentTypeId, string DocumentNumber, DateTime IssuedDate, string OrganizationName, string? QualificationAwardedName, string? SpecialtyName, string? ProgramName, TimeSpan? TotalHours);
}
