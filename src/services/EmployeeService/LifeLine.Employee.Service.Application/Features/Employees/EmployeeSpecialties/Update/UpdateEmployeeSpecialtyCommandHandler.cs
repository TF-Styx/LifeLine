using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Update
{
    public sealed class UpdateEmployeeSpecialtyCommandHandler
        (
            IWriteContext сontext,
            IEmployeeRepository кepository
        ) : IRequestHandler<UpdateEmployeeSpecialtyCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateEmployeeSpecialtyCommand request, CancellationToken cancellationToken)
        {
            var employee = await кepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");
                
            employee.RemoveSpecialty(request.SpecialtyIdsOld);
            employee.AddSpecialty(request.SpecialtyIdsNew);

            await сontext.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
