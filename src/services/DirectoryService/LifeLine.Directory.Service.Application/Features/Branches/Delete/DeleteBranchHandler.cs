using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Errors;
using Terminex.Common.Primitives;
using Terminex.Common.Results;

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
            var branch = await repository.GetByIdAsync(request.Id);

            if (branch == null)
                return Error.NotFound("Запись филиала не найдена!");

            var hasDepartments = await context.Departments.AnyAsync(x => x.BranchId == branch.Id && !x.IsDeleted);

            if (hasDepartments)
                return new Error(AppErrors.ExistDependentData, $"У филиала - `{branch.Name}`, имеются отделы!");

            branch.Delete();

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
