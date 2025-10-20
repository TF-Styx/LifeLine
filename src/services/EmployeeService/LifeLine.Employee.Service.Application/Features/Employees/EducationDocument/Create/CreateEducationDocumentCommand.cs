using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Create
{
    public sealed record CreateEducationDocumentCommand(Guid EmployeeId, Guid EducationLevelId, Guid DocumentTypeId, string DocumentNumber, DateTime IssuedDate, string OrganizationName, string? QualificationAwardedName, string? SpecialtyName, string? ProgramName, TimeSpan? TotalHours) : IRequest<Result>;
}
