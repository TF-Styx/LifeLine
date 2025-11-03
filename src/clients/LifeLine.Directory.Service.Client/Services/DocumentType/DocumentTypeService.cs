using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.DocumentType
{
    public sealed class DocumentTypeService(HttpClient httpClient) : BaseHttpService<DocumentTypeResponse, string>(httpClient, "api/document-types"), IDocumentTypeService;
}
