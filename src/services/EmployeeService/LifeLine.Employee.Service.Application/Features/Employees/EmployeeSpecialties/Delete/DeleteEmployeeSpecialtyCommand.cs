using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Delete
{
    public sealed record DeleteEmployeeSpecialtyCommand(Guid EmployeeId, Guid SpecialtyId) : IRequest<Result<Nothing>>;
}
