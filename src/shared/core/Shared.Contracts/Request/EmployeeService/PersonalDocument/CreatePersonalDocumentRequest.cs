namespace Shared.Contracts.Request.EmployeeService.PersonalDocument
{
    public sealed record CreatePersonalDocumentRequest(Guid DocumentTypeId, string DocumentNumber, string? DocumentSeries, string BucketName, string FileName);
}
