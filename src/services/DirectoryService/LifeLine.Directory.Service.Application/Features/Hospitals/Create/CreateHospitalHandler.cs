using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Create
{
    public sealed class CreateHospitalHandler
        (
            IDirectoryContext context,
            IHospitalRepository repository
        ) : IRequestHandler<CreateHospitalCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = Hospital.Create
            (
                request.Name,
                request.Description,
                request.Phone,
                request.Email,
                Address.Create
                (
                    request.Address.PostalCode,
                    request.Address.Region,
                    request.Address.City,
                    request.Address.Street,
                    request.Address.Building,
                    request.Address.Apartment
                )
            );

            await repository.AddAsync(hospital, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return hospital.Id.ToString();
        }
    }
}
