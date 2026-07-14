using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.Branches.Get.GetAll
{
    public sealed class GetAllBranchHandler(IDirectoryContext context) : IRequestHandler<GetAllBranchQuery, List<BranchResponse>>
    {
        public async Task<List<BranchResponse>> Handle(GetAllBranchQuery request, CancellationToken cancellationToken)
            => await context.Branches
                .AsNoTracking()
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
                            x.Address.Street,
                            x.Address.Building,
                            x.Address.Apartment
                        )
                    )
                ).ToListAsync(cancellationToken);
    }
}
