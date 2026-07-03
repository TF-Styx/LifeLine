using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.CreateMany
{
    public sealed record CreateManyEducationDocumentsCommand
    (
        Guid EmployeeId, 
        List<CreateDataEducationDocumentCommand> EducationDocuments
    ) : IRequest<Result<Nothing>>;

    public sealed record CreateDataEducationDocumentCommand
    (
        Guid EducationLevelId, 
        Guid DocumentTypeId, 
        string DocumentNumber, 
        DateTime IssuedDate, 
        tring OrganizationName, 
        string? QualificationAwardedName, 
        string? SpecialtyName, 
        string? ProgramName, 
        TimeSpan? TotalHours, 
        string? BucketName, 
        string? FileName
    );
}
