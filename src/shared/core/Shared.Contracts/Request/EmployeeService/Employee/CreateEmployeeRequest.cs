namespace Shared.Contracts.Request.EmployeeService.Employee
{
    public sealed record CreateEmployeeRequest
        (
            string Surname, string Name, string? Patronymic, string GenderId, 
            List<CreateEmployeePersonalDocumentRequest>? PersonalDocuments, 
            CreateContactInformationRequest? ContactInformation,
            List<CreateEducationDocumentRequest>? EducationDocument,
            List<CreateWorkPermitRequest>? WorkPermit, 
            List<CreateAssignmentRequest>? AssignmentContract
        );

    public sealed record CreateEmployeePersonalDocumentRequest(Guid DocumentTypeId, string Number, string? Series);

    public sealed record CreateContactInformationRequest(string PersonalPhone, string? CorporatePhone, string PersonalEmail, string? CorporateEmail, string PostalCode, string Region, string City, string Street, string Building, string? Apartment);

    public sealed record CreateEducationDocumentRequest(Guid EducationLevelId, Guid DocumentTypeId, string DocumentNumber, DateTime IssuedDate, string OrganizationName, string? QualificationAwardedName, string? SpecialtyName, string? ProgramName, TimeSpan? TotalHours);

    public sealed record CreateWorkPermitRequest(string WorkPermitName, string? DocumentSeries, string WorkPermitNumber, string? ProtocolNumber, string SpecialtyName, string IssuingAuthority, DateTime IssueDate, DateTime ExpiryDate, /*FileInput? FileKey, */Guid PermitTypeId, Guid AdmissionStatusId);

    public sealed record CreateAssignmentRequest(Guid PositionId, Guid DepartmentId, Guid? ManagerId, DateTime HireDate, DateTime TerminationDate, Guid StatusId, CreateAssignmentContractRequest Contract);

    public sealed record CreateAssignmentContractRequest(Guid EmployeeTypeId, string ContractNumber, DateTime StartDate, DateTime EndDate, decimal Salary/*, IFormFile? FileKey*/);
}
