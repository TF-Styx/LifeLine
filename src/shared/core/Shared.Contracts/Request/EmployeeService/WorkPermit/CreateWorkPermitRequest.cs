namespace Shared.Contracts.Request.EmployeeService.WorkPermit
{
    public sealed record CreateWorkPermitRequest(string WorkPermitName, string? DocumentSeries, string WorkPermitNumber, string? ProtocolNumber, string SpecialtyName, string IssuingAuthority, DateTime IssueDate, DateTime ExpiryDate, /*FileInput? FileKey, */Guid PermitTypeId, Guid AdmissionStatusId);
}
