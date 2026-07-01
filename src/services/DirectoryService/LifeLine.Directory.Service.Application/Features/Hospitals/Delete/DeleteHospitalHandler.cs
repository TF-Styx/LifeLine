using MediatR;
using Terminex.Common.Results;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Delete
{
    public sealed class DeleteHospitalHandler
        (
            IDirectoryContext context, 
            IHospitalRepository repository, 
            ILogger<DeleteHospitalHandler> logger
        ) : IRequestHandler<DeleteHospitalCommand, Result>
    {
        public async Task<Result> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hospital = await repository.GetByIdAsync(request.Id);

                if (hospital == null)
                    return Result.Failure(Error.NotFound("Запись больницы не найдена!"));

                repository.Remove(hospital);

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Ошибка при удалении Hospital!");

                return Result.Failure(Error.Server("Ошибка сервера при сохранении!"));
            }
        }
    }
}
