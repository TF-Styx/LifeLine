using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.Delete
{
    public sealed record DeleteEmployeeCommand(Guid Id) : IRequest<Result<Nothing>>;
}
