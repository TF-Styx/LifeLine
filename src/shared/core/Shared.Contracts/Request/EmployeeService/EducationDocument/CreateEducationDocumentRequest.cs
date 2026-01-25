namespace Shared.Contracts.Request.EmployeeService.EducationDocument
{
    public sealed record CreateEducationDocumentRequest(Guid EducationLevelId, Guid DocumentTypeId, string DocumentNumber, DateTime IssuedDate, string OrganizationName, string? QualificationAwardedName, string? SpecialtyName, string? ProgramName, double? TotalHours);
}
