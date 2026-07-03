using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using Shared.Contracts.Request.Shared;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Create
{
    public sealed record CreatePersonalDocumentCommand
        (
            Guid EmployeeId, 
            Guid DocumentTypeId, 
            string DocumentNumber, 
            string? DocumentSeries, 
            FileInput? FileInput
        ) : IRequest<Result<Nothing>>;
}
