using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Update
{
    public sealed record UpdatePersonalDocumentCommand
        (
            string Id, 
            string EmployeeId, 
            string DocumentTypeId, 
            string DocumentNumber, 
            string? DocumentSeries
        ) : IRequest<Result>;
}
