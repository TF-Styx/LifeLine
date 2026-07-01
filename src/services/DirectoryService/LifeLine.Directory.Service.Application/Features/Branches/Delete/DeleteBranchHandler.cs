using MediatR;
using Terminex.Common.Results;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Branches.Delete
{
    public sealed class DeleteBranchHandler
        (
            IDirectoryContext context,
            IBranchRepository repository, 
            ILogger<DeleteBranchHandler> logger
        ) : IRequestHandler<DeleteBranchCommand, Result>
    {
        public async Task<Result> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var branch = await repository.GetByIdAsync(request.DepartmentId);

                if (branch == null)
                    return Result.Failure(Error.NotFound("Запись филиала не найдена!"));

                repository.Remove(branch);

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Ошибка при удалении Branch!");

                return Result.Failure(Error.Server("Ошибка сервера при сохранении!"));
            }
        }
    }
}
