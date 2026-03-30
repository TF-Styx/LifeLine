using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Create.CreateMany
{
    public sealed record CreateManyEmployeeSpecialtiesCommand(Guid EmployeeId, List<CreateManyDataEmployeeSpecialtiesCommand> Specialties) : IRequest<Result>;
    public sealed record CreateManyDataEmployeeSpecialtiesCommand(Guid SpecialtyId);
}
