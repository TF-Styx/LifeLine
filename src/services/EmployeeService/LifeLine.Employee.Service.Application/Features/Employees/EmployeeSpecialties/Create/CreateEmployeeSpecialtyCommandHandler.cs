using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create
{
    public sealed class CreateEmployeeSpecialtyCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateEmployeeSpecialtyCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateEmployeeSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.AddSpecialty(request.SpecialtyId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
