using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAll
{
    public sealed class GetAllPositionByDepartmentQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllPositionByDepartmentQuery, List<PositionResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<PositionResponse>> Handle(GetAllPositionByDepartmentQuery request, CancellationToken cancellationToken)
            => await _context.Positions
                .Where(x => x.DepartmentId == request.DepartmentId)
                    .Select(x => new PositionResponse(x.Name, x.Description))
                        .ToListAsync(cancellationToken);
    }
}
