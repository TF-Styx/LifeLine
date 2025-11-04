using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.PermitTypes.Get.GetAll
{
    public sealed class GetAllPermitTypeQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllPermitTypeQuery, List<PermitTypeResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<PermitTypeResponse>> Handle(GetAllPermitTypeQuery request, CancellationToken cancellationToken)
            => await _context.PermitTypes.Select(x => new PermitTypeResponse(x.Id.ToString(), x.PermitTypeName)).ToListAsync(cancellationToken);
    }
}
