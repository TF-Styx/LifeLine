using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.DocumentTypes.Get.GetAll
{
    public sealed class GetAllDocumentTypeQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllDocumentTypeQuery, List<DocumentTypeResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<DocumentTypeResponse>> Handle(GetAllDocumentTypeQuery request, CancellationToken cancellationToken)
            => await _context.DocumentTypes.Select(x => new DocumentTypeResponse(x.Id, x.DocumentTypeName)).ToListAsync(cancellationToken);
    }
}
