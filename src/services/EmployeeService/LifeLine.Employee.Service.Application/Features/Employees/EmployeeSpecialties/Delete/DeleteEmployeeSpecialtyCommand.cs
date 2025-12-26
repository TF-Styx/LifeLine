using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Delete
{
    public sealed record DeleteEmployeeSpecialtyCommand(Guid EmployeeId, Guid SpecialtyId) : IRequest<Result>;
}
