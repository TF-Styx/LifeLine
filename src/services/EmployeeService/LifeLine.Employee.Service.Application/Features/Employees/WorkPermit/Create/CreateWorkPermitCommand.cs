using MediatR;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Create
{
    public sealed record CreateWorkPermitCommand(Guid EmployeeId, string WorkPermitName, string? DocumentSeries, string WorkPermitNumber, string? ProtocolNumber, string SpecialtyName, string IssuingAuthority, DateTime IssueDate, DateTime ExpiryDate, /*ImageKey? FileKey, */Guid PermitTypeId, Guid AdmissionStatusId) : IRequest<Result>;
}
