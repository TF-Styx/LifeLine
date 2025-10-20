using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Add
{
    public sealed record CreateEmployeeSpecialtyCommand(Guid EmployeeId, Guid SpecialtyId) : IRequest<Result>;
}
