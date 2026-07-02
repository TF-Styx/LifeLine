using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Update
{
    public sealed class UpdateStatusCommandHandler
        (
            IDirectoryContext context,
            IStatusRepository repository
        ) : IRequestHandler<UpdateStatusCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var status = await repository.GetByIdAsync(request.Id);

            if (status == null)
                return Error.NotFound("Запись статуса не найдена!");

            status.UpdateStatusName(DirectoryName.Create(request.Name));
            status.UpdateStatusDescription(Description.Create(request.Description));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
