using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Branches.Create
{
    public sealed class CreateBranchHandler
        (
            IDirectoryContext context,
            IBranchRepository repository
        ) : IRequestHandler<CreateBranchCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = Branch.Create
            (
                request.Name,
                request.Description,
                request.Phone,
                request.Email,
                request.HospitalId,
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

            await repository.AddAsync(branch, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return branch.Id.ToString();
        }
    }
}
