using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Departments.Get.GetAll
{
    public sealed class GetAllDepartmentQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllDepartmentQuery, List<DepartmentResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<DepartmentResponse>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
            => await _context.Departments.Select(x => new DepartmentResponse(x.Id, x.Name, x.Description)).ToListAsync(cancellationToken);
    }
}
