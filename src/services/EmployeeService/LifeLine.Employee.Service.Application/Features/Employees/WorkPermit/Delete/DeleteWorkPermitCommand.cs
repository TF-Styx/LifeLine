using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Delete
{
    public sealed record DeleteWorkPermitCommand(Guid EmployeeId, Guid WorkPermitId) : IRequest<Result<Nothing>>;
}
