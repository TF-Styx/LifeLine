using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.Departments.Get.GetAllByBranchId
{
    public sealed class GetAllByBranchIdHandler(IDirectoryContext context) : IRequestHandler<GetAllByBranchIdQuery, List<DepartmentResponse>>
    {
        public async Task<List<DepartmentResponse>> Handle(GetAllByBranchIdQuery request, CancellationToken cancellationToken)
            => await context.Departments
                .AsNoTracking()
                .Where(x => x.BranchId == request.BranchId)
                .Select(x => new DepartmentResponse(x.Id.ToString(), x.Name, x.Description!, x.Building, x.BranchId.ToString()))
                .ToListAsync();
    }
}
