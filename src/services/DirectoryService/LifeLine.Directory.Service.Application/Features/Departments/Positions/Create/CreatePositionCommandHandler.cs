using MediatR;
using Terminex.Common.Results;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Create
{
    public sealed class CreatePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository
        ) : IRequestHandler<CreatePositionCommand, Result<string>>
    {
       public async Task<Result<string>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var department = await repository.GetByIdAsync(request.Id);

            if (department == null)
                return Error.NotFound("Запись департамента не найдена!");

            var positionId = department.AddPositions(request.Name, request.Description);

            await context.SaveChangesAsync(cancellationToken);

            return positionId;
        }
    }
}
