using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Get.GetAll
{
    public sealed class GetAllHospitalHandler(IDirectoryContext context) : IRequestHandler<GetAllHospitalQuery, List<HospitalResponse>>
    {
        public async Task<List<HospitalResponse>> Handle(GetAllHospitalQuery request, CancellationToken cancellationToken)
            => await context.Hospitals
                .AsNoTracking()
                .Select
                (
                    x => new HospitalResponse
                    (
                        x.Id.ToString(),
                        x.Name,
                        x.Description!,
                        x.Phone,
                        x.Email,
                        new HospitalDataAddressResponse
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
