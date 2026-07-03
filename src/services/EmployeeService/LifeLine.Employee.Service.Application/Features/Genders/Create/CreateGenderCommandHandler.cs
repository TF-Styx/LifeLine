using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Genders.Create
{
    public sealed class CreateGenderCommandHandler
        (
            IWriteContext context, 
            IGenderRepository repository
        ) : IRequestHandler<CreateGenderCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateGenderCommand request, CancellationToken cancellationToken)
        {
            var entity = Gender.Create(request.Name);

            await repository.AddAsync(entity, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
