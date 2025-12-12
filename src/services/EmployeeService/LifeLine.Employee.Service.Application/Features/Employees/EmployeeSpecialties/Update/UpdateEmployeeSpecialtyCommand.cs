using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Update
{
    public sealed record UpdateEmployeeSpecialtyCommand(Guid EmployeeId, Guid SpecialtyIdsOld, Guid SpecialtyIdsNew) : IRequest<Result>;
}
