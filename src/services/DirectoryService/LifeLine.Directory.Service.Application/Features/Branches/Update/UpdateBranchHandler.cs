using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Application.Common.Repository;

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
            branch.UpdateAddress(Address.Create(request.Address.PostalCode, request.Address.Region, request.Address.City, request.Address.Street));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
