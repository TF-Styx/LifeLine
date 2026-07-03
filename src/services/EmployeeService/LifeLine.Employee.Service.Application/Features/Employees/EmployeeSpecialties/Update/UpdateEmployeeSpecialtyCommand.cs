using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Update
{
    public sealed record UpdateEmployeeSpecialtyCommand(Guid EmployeeId, Guid SpecialtyIdsOld, Guid SpecialtyIdsNew) : IRequest<Result<Nothing>>;
}
