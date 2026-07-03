using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Application.Common.Repository;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Update
{
    public sealed class UpdateHospitalHandler
        (
            IDirectoryContext context, 
            IHospitalRepository repository
        ) : IRequestHandler<UpdateHospitalCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await repository.GetByIdAsync(request.Id);

            if (hospital == null)
                return Error.NotFound("Запись больницы не найдена!");

            hospital.UpdateName(DirectoryName.Create(request.Name));
            hospital.UpdateDescription(!string.IsNullOrWhiteSpace(request.Description) ? Description.Create(request.Description) : null);
            hospital.UpdatePhone(Phone.Create(request.Phone));
            hospital.UpdateEmail(Email.Create(request.Email));
            hospital.UpdateAddress(Address.Create(request.Address.PostalCode, request.Address.Region, request.Address.City, request.Address.Street));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
