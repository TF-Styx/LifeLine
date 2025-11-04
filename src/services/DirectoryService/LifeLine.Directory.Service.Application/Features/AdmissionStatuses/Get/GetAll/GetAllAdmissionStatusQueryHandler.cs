using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Get.GetAll
{
    public sealed class GetAllAdmissionStatusQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllAdmissionStatusQuery, List<AdmissionStatusResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<AdmissionStatusResponse>> Handle(GetAllAdmissionStatusQuery request, CancellationToken cancellationToken)
            => await _context.AdmissionStatuses.Select(x => new AdmissionStatusResponse(x.Id.ToString(), x.AdmissionStatusName)).ToListAsync(cancellationToken);
    }
}
