using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Update
{
    public sealed record UpdateEmployeeTypeCommand(Guid Id, string Name, string Description) : IRequest<Result<Nothing>>;
}
