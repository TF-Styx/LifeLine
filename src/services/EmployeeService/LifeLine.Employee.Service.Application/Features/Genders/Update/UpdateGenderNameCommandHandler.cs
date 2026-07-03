using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Genders.Update
{
    public sealed class UpdateGenderNameCommandHandler
        (
            IWriteContext context, 
            IGenderRepository repository
        ) : IRequestHandler<UpdateGenderNameCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateGenderNameCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return Error.NotFound("Запись не найдена!");

            entity.UpdateName(GenderName.Create(request.Name));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
