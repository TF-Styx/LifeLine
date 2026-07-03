using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Delete
{
    public sealed record DeleteEmployeeTypeCommand(Guid Id) : IRequest<Result<Nothing>>;
}
