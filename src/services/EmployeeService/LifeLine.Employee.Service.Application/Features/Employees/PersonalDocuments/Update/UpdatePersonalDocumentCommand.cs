using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Update
{
    public sealed record UpdatePersonalDocumentCommand
    (
        string Id, 
        string EmployeeId, 
        string DocumentTypeId, 
        string DocumentNumber, 
        string? DocumentSeries, 
        string? BucketName, 
        string? FileName
    ) : IRequest<Result<Nothing>>;
}
