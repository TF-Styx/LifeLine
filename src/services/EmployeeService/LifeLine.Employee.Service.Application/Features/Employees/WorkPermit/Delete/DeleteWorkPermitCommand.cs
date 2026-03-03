using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Delete
{
    public sealed record DeleteWorkPermitCommand(Guid EmployeeId, Guid WorkPermitId) : IRequest<Result>;
}
