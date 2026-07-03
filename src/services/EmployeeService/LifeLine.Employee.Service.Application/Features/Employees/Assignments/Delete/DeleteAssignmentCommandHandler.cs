using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Delete
{
    public sealed class DeleteAssignmentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<DeleteAssignmentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.DeleteAssignmentContract(request.AssignmentId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
