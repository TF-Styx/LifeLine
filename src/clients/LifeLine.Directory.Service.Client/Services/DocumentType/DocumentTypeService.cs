using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.DocumentType
{
    public sealed class DocumentTypeService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<DocumentTypeResponse, string>(httpClient, "api/document-types", options.Value), IDocumentTypeService;
}
