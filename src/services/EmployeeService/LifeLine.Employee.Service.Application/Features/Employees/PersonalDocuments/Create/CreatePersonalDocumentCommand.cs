using MediatR;
using Shared.Contracts.Request.Shared;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Create
{
    public sealed record CreatePersonalDocumentCommand(Guid EmployeeId, Guid DocumentTypeId, string DocumentNumber, string? DocumentSeries, FileInput? FileInput) : IRequest<Result>;
}
