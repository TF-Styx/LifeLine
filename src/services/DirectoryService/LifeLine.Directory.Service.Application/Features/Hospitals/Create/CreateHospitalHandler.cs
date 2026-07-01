using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Create
{
    public sealed class CreateHospitalHandler
        (
            IDirectoryContext context,
            IHospitalRepository repository,
            ILogger<CreateHospitalHandler> logger
        ) : IRequestHandler<CreateHospitalCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateHospitalCommand request, CancellationToken cancellationToken)
        {
            try
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
                        request.Address.Street
                    )
                );

                await repository.AddAsync(hospital, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return Result<string>.Success(hospital.Id.ToString());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Ошибка при создании Department!");

                return Result<string>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
