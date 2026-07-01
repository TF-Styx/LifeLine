using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.Branches.Get.GetAllByHospitalId
{
    public sealed class GetAllByHospitalIdHandler(IDirectoryContext context) : IRequestHandler<GetAllByHospitalIdQuery, List<BranchResponse>>
    {
        public async Task<List<BranchResponse>> Handle(GetAllByHospitalIdQuery request, CancellationToken cancellationToken)
            => await context.Branches
                .AsNoTracking()
                .Where(x => x.HospitalId == request.HospitalId)
                .Select
                (
                    x => new BranchResponse
                    (
                        x.Id.ToString(),
                        x.Name,
                        x.Description!,
                        x.Phone,
                        x.Email,
                        x.HospitalId.ToString(),
                        new BranchDataAddressResponse
                        (
                            x.Address.PostalCode,
                            x.Address.Region,
                            x.Address.City,
                            x.Address.Street
                        )
                    )
                ).ToListAsync(cancellationToken);
    }
}
