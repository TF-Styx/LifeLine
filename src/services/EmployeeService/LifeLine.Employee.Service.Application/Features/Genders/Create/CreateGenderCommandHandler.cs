using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Create
{
    public sealed class CreateGenderCommandHandler
        (
            IWriteContext context, 
            IGenderRepository repository
        ) : IRequestHandler<CreateGenderCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateGenderCommand request, CancellationToken cancellationToken)
        {
            var gender = Gender.Create(request.Name);

            await repository.AddAsync(gender, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return gender.Id.ToString();
        }
    }
}
