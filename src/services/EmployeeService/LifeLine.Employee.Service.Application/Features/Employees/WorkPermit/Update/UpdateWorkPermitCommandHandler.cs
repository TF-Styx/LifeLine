using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Update
{
    public sealed class UpdateWorkPermitCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<UpdateWorkPermitCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateWorkPermitCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.UpdateNameWP(request.Id, request.WorkPermitName);
            employee.UpdateDocumentSeriesWP(request.Id, request.DocumentSeries);
            employee.UpdateDocumentNumberWP(request.Id, request.WorkPermitNumber);
            employee.UpdateProtocolNumberWP(request.Id, request.ProtocolNumber);
            employee.UpdateSpecialtyNameWP(request.Id, request.SpecialtyName);
            employee.UpdateIssuingAuthorityWP(request.Id, request.IssuingAuthority);
            employee.UpdateIssueDateWP(request.Id, request.IssueDate);
            employee.UpdateExpiryDateWP(request.Id, request.ExpiryDate);
            employee.UpdateFileKeyWorkPermit(request.Id, request.BucketName, request.FileName);
            employee.UpdatePermitTypeIdWP(request.Id, request.PermitTypeId);
            employee.UpdateAdmissionStatusIdWP(request.Id, request.AdmissionStatusId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
