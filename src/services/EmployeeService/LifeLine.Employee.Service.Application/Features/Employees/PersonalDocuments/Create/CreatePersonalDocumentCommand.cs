using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Create
{
    public sealed record CreatePersonalDocumentCommand
        (
            Guid EmployeeId, 
            Guid DocumentTypeId, 
            string DocumentNumber, 
            string? DocumentSeries,
            string BucketName,
            string FileName
        ) : IRequest<Result<string>>;
}
