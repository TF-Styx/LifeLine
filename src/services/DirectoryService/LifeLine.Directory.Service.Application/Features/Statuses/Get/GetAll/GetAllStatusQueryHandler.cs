using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Get.GetAll
{
    public sealed class GetAllStatusQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllStatusQuery, List<StatusResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<StatusResponse>> Handle(GetAllStatusQuery request, CancellationToken cancellationToken)
            => await _context.Statuses.Select(x => new StatusResponse(x.Id, x.Name, x.Description)).ToListAsync(cancellationToken);
    }
}
