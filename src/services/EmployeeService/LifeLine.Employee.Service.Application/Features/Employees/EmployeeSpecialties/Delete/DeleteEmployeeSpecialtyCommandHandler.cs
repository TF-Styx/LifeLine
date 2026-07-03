using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Delete
{
    public sealed class DeleteEmployeeSpecialtyCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<DeleteEmployeeSpecialtyCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteEmployeeSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee is null)
                return Error.NotFound("Пользователь не найден!");

            employee.RemoveSpecialty(request.SpecialtyId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
