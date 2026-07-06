using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Create
{
    public sealed class CreateEmployeeTypeCommandHandler
        (
            IWriteContext context,
            IEmployeeTypeRepository repository
        ) : IRequestHandler<CreateEmployeeTypeCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            var employeeType = EmployeeType.Create(request.Name, request.Description);

            await repository.AddAsync(employeeType, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return employeeType.Id.ToString();
        }
    }
}
