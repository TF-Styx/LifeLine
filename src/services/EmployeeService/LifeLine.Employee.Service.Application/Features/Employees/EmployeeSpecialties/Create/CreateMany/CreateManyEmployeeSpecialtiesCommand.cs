using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create.CreateMany
{
    public sealed record CreateManyEmployeeSpecialtiesCommand
    (
        Guid EmployeeId, 
        List<CreateManyDataEmployeeSpecialtiesCommand> Specialties
    ) : IRequest<Result<Nothing>>;
    public sealed record CreateManyDataEmployeeSpecialtiesCommand(Guid SpecialtyId);
}
