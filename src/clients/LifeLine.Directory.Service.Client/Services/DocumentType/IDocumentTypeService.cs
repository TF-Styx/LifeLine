using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.DocumentType
{
    public interface IDocumentTypeService : IDocumentTypeReadOnlyService, IBaseWriteHttpService<DocumentTypeResponse, string>;
}
