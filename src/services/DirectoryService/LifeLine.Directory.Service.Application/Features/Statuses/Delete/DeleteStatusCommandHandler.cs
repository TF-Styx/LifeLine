using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Delete
{
    public sealed class DeleteStatusCommandHandler
        (
            IDirectoryContext context,
            IStatusRepository repository
        ) : IRequestHandler<DeleteStatusCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
        {
            var status = await repository.GetByIdAsync(request.Id);

            if (status == null)
                return Error.NotFound("Запись статуса не найдена!");

            repository.Remove(status);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
