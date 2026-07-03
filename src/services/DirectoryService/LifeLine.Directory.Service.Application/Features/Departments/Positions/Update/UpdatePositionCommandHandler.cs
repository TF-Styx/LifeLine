using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Update
{
    public sealed class UpdatePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository
        ) : IRequestHandler<UpdatePositionCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.DepartmentId);

            if (department == null)
                return Error.NotFound("Запись департамента не найдена!");

            department.UpdatePositionName(request.PositionId, request.Name);
            department.UpdatePositionDescription(request.PositionId, request.Description);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
