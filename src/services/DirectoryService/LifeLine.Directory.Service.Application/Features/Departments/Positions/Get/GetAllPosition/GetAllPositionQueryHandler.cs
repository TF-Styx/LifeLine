using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAllPosition
{
    public sealed class GetAllPositionQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllPositionQuery, List<PositionResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<PositionResponse>> Handle(GetAllPositionQuery request, CancellationToken cancellationToken)
            => await _context.Positions
                .Select(x => new PositionResponse(x.Id, x.Name, x.Description))
                    .ToListAsync(cancellationToken);
    }
}
