using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Genders.Delete
{
    public sealed class DeleteGenderCommandHandler
        (
            IWriteContext context,
            IGenderRepository repository
        ) : IRequestHandler<DeleteGenderCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteGenderCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return Error.NotFound("Запись не найдена!");

            repository.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
