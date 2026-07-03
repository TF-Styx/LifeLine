using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.SoftDelete
{
    public sealed record SoftDeleteEmployeeCommand(Guid EmployeeId) : IRequest<Result<Nothing>>;
}
