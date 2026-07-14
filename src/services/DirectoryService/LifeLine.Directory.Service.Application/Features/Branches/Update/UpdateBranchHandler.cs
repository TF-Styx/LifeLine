using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.ValueObjects;
using MediatR;
using Shared.Domain.ValueObjects;
using Terminex.Common.Primitives;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Branches.Update
{
    public sealed class UpdateBranchHandler
        (
            IDirectoryContext context,
            IBranchRepository repository
        ) : IRequestHandler<UpdateBranchCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = await repository.GetByIdAsync(request.BranchId);

            if (branch == null)
                return Error.NotFound("Запись больницы не найдена!");

            branch.UpdateName(DirectoryName.Create(request.Name));
            branch.UpdateDescription(!string.IsNullOrWhiteSpace(request.Description) ? Description.Create(request.Description) : null);
            branch.UpdatePhone(Phone.Create(request.Phone));
            branch.UpdateEmail(Email.Create(request.Email));
            branch.UpdateHospitalId(HospitalId.Create(request.HospitalId));
            branch.UpdateAddress
            (
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

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
