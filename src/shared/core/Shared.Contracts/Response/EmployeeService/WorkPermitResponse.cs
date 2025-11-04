namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record WorkPermitResponse(string EmployeeId, string WorkPermitName, string? DocumentSeries, string WorkPermitNumber, string? ProtocolNumber, string SpecialtyName, string IssuingAuthority, DateTime IssueDate, DateTime ExpiryDate, /*FileInput? FileKey, */string PermitTypeId, string AdmissionStatusId);
}
