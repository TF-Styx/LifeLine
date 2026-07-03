using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Branches.Delete
{
    public sealed class DeleteBranchHandler
        (
            IDirectoryContext context,
            IBranchRepository repository
        ) : IRequestHandler<DeleteBranchCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = await repository.GetByIdAsync(request.DepartmentId);

            if (branch == null)
                return Error.NotFound("Запись филиала не найдена!");

            repository.Remove(branch);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
