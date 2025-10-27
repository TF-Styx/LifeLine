using Shared.Contracts.Request.Shared;

namespace LifeLine.Employee.Service.Api.Models.Request
{
    public sealed record CreatePersonalDocumentRequest(Guid DocumentTypeId, string DocumentNumber, string? DocumentSeries, IFormFile? FileInput);
}
