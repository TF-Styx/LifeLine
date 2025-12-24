using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Delete
{
    public sealed record DeletePersonalDocuemtnCommand(Guid PersonalDocumentId, Guid EmployeeId) : IRequest<Result>;
}
