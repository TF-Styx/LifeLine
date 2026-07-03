using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.Delete
{
    public sealed class DeleteEmployeeCommandHandler
        (
            IWriteContext context, 
            IEmployeeRepository repository
        ) : IRequestHandler<DeleteEmployeeCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return Error.NotFound("Запись не найдена!");

            repository.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
