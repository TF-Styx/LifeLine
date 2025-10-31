using MediatR;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.DocumentTypes.Get.GetAll
{
    public sealed record GetAllDocumentTypeQuery : IRequest<List<DocumentTypeResponse>>;
}
