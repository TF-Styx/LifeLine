using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create.CreateMany
{
    public sealed class CreateManyEmployeeSpecialtiesCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateManyEmployeeSpecialtiesCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateManyEmployeeSpecialtiesCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            foreach (var item in request.Specialties)
                employee.AddSpecialty(item.SpecialtyId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
