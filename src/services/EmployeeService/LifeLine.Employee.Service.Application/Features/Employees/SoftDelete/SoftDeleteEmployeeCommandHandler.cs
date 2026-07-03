using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.SoftDelete
{
    public sealed class SoftDeleteEmployeeCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<SoftDeleteEmployeeCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(SoftDeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.Deactivate();

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
