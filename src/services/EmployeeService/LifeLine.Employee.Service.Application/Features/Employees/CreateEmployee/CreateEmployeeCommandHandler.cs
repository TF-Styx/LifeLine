using MediatR;
using Terminex.Common.Results;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.CreateEmployee
{
    public sealed class CreateEmployeeCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateEmployeeCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = Domain.Models.Employee.Create(request.Surname, request.Name, request.Patronymic, request.GenderId);

            await repository.AddAsync(employee, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return employee.Id.ToString();
        }
    }
}
