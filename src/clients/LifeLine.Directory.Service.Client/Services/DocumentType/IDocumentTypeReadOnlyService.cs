using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.DocumentType
{
    public interface IDocumentTypeReadOnlyService : IBaseReadHttpService<DocumentTypeResponse, string>;
}
