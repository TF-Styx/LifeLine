using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Delete
{
    public sealed record DeleteEducationDocumentCommand(Guid EducationDocumentId, Guid EmployeeId) : IRequest<Result<Nothing>>;
}
