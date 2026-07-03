using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.CreateMany
{
    public sealed record CreateManyPersonalDocumentsCommand
(
    Guid EmployeeId, 
    List<CreateDataPersonalDocumentCommand> PersonalDocuments
) : IRequest<Result<Nothing>>;

    public sealed record CreateDataPersonalDocumentCommand
    (
        Guid DocumentTypeId, 
        string DocumentNumber, 
        string? DocumentSeries, 
        string? BucketName, 
        string? FileName
    );
}
