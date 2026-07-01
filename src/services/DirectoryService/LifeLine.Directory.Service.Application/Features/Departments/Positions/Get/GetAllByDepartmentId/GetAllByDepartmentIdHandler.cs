using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAllByDepartmentId
{
    public sealed class GetAllByDepartmentIdHandler(IDirectoryContext context) : IRequestHandler<GetAllByDepartmentIdQuery, List<PositionResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<PositionResponse>> Handle(GetAllByDepartmentIdQuery request, CancellationToken cancellationToken)
            => await _context.Positions
                .Where(x => x.DepartmentId == request.DepartmentId)
                    .Select(x => new PositionResponse(x.Id.ToString(), x.Name, x.Description!))
                        .ToListAsync(cancellationToken);
    }
}
