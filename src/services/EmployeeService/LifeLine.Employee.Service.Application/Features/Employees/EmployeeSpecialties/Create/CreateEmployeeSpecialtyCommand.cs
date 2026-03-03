using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create
{
    public sealed record CreateEmployeeSpecialtyCommand(Guid EmployeeId, Guid SpecialtyId) : IRequest<Result>;
}
