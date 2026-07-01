using MediatR;
using Terminex.Common.Results;
using Shared.Kernel.Exceptions;
using Shared.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Update
{
    public sealed class UpdateHospitalHandler
        (
            IDirectoryContext context, 
            IHospitalRepository repository, 
            ILogger<UpdateHospitalHandler> logger
        ) : IRequestHandler<UpdateHospitalCommand, Result>
    {
        public async Task<Result> Handle(UpdateHospitalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hospital = await repository.GetByIdAsync(request.Id);

                if (hospital == null)
                    return Result.Failure(Error.NotFound("Запись больницы не найдена!"));

                hospital.UpdateName(DirectoryName.Create(request.Name));
                hospital.UpdateDescription(!string.IsNullOrWhiteSpace(request.Description) ? Description.Create(request.Description) : null);
                hospital.UpdatePhone(Phone.Create(request.Phone));
                hospital.UpdateEmail(Email.Create(request.Email));
                hospital.UpdateAddress(Address.Create(request.Address.PostalCode, request.Address.Region, request.Address.City, request.Address.Street));

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(Error.Create(domainEX.Message));
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Ошибка при обновлении Hospital!");

                return Result.Failure(Error.Server("Ошибка сервера при сохранении!"));
            }
        }
    }
}
