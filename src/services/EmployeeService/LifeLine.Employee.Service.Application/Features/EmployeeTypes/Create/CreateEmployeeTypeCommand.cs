using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Create
{
    public sealed record CreateEmployeeTypeCommand(string Name, string Description) : IRequest<Result>;
}
