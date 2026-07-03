using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Delete
{
    public sealed class DeleteEmployeeTypeCommandHandler
        (
            IWriteContext context,
            IEmployeeTypeRepository repository
        ) : IRequestHandler<DeleteEmployeeTypeCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            var employeeType = await repository.GetByIdAsync(request.Id);

            if (employeeType == null)
                return Error.NotFound("Запись типа занятости не найдена!");

            repository.Remove(employeeType);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
