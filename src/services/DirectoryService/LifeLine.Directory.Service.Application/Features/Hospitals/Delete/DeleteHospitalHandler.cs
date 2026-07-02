using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Delete
{
    public sealed class DeleteHospitalHandler
        (
            IDirectoryContext context, 
            IHospitalRepository repository
        ) : IRequestHandler<DeleteHospitalCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
        {
            var hospital = await repository.GetByIdAsync(request.Id);

            if (hospital == null)
                return Error.NotFound("Запись больницы не найдена!");

            repository.Remove(hospital);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
