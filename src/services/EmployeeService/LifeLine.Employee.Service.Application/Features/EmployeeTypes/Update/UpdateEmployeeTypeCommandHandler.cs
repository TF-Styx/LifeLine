using MediatR;
using Terminex.Common.Results;
using Shared.Domain.ValueObjects;
using Terminex.Common.Primitives;
using LifeLine.Employee.Service.Domain.ValueObjects.EmployeeType;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Update
{
    public sealed class UpdateEmployeeTypeCommandHandler
        (
            IWriteContext context,
            IEmployeeTypeRepository repository
        ) : IRequestHandler<UpdateEmployeeTypeCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateEmployeeTypeCommand request, CancellationToken cancellationToken)
        {
            var employeeType = await repository.GetByIdAsync(request.Id);

            if (employeeType == null)
                return Error.NotFound("Запись типа занятости не найдена!");

            employeeType.UpdateName(EmployeeTypeName.Create(request.Name));
            employeeType.UpdateDescription(Description.Create(request.Description));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
