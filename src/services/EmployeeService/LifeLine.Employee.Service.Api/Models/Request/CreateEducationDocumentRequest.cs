namespace LifeLine.Employee.Service.Api.Models.Request
{
    public sealed record CreateEducationDocumentRequest(Guid EducationLevelId, Guid DocumentTypeId, string DocumentNumber, DateTime IssuedDate, string OrganizationName, string? QualificationAwardedName, string? SpecialtyName, string? ProgramName, double? TotalHours);
}
