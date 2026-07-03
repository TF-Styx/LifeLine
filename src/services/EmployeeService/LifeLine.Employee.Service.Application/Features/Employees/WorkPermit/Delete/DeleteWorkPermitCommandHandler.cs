using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Delete
{
    public sealed class DeleteWorkPermitCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<DeleteWorkPermitCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteWorkPermitCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.DeleteWorkPermit(request.WorkPermitId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
