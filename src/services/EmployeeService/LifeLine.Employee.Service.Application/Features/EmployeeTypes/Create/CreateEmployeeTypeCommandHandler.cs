using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Create
{
    public sealed class CreateEmployeeTypeCommandHandler
        (
            IWriteContext context,
            IEmployeeTypeRepository repository
        ) : IRequestHandler<CreateEmployeeTypeCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            var employeeType = EmployeeType.Create(request.Name, request.Description);

            await repository.AddAsync(employeeType, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
