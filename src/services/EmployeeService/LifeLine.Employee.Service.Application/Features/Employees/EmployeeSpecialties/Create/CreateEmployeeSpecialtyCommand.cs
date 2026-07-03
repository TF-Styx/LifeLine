using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create
{
    public sealed record CreateEmployeeSpecialtyCommand(Guid EmployeeId, Guid SpecialtyId) : IRequest<Result<Nothing>>;
}
