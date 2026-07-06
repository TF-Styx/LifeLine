using MediatR;
using Terminex.Common.Results;
using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Specialties.Create
{
    public sealed class CreateSpecialtyCommandHandler
        (
            IWriteContext context,
            ISpecialtyRepository repository
        ) : IRequestHandler<CreateSpecialtyCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var specialty = Specialty.Create(request.SpecialtyName, request.Description);

            await repository.AddAsync(specialty, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return specialty.Id.ToString();
        }
    }
}
