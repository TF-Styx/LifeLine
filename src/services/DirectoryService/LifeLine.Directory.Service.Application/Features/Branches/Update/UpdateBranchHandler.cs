using MediatR;
using Terminex.Common.Results;
using Shared.Kernel.Exceptions;
using Shared.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Branches.Update
{
    public sealed class UpdateBranchHandler
        (
            IDirectoryContext context,
            IBranchRepository repository, 
            ILogger<UpdateBranchHandler> logger
        ) : IRequestHandler<UpdateBranchCommand, Result>
    {
        public async Task<Result> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var branch = await repository.GetByIdAsync(request.BranchId);

                if (branch == null)
                    return Result.Failure(Error.NotFound("Запись больницы не найдена!"));

                branch.UpdateName(DirectoryName.Create(request.Name));
                branch.UpdateDescription(!string.IsNullOrWhiteSpace(request.Description) ? Description.Create(request.Description) : null);
                branch.UpdatePhone(Phone.Create(request.Phone));
                branch.UpdateEmail(Email.Create(request.Email));
                branch.UpdateHospitalId(HospitalId.Create(request.HospitalId));
                branch.UpdateAddress(Address.Create(request.Address.PostalCode, request.Address.Region, request.Address.City, request.Address.Street));

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(Error.Create(domainEX.Message));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Ошибка при обновлении Branch!");

                return Result.Failure(Error.Server("Ошибка сервера при сохранении!"));
            }
        }
    }
}
