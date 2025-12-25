using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Delete
{
    public sealed record DeleteEducationDocumentCommand(Guid EducationDocumentId, Guid EmployeeId) : IRequest<Result>;
}
