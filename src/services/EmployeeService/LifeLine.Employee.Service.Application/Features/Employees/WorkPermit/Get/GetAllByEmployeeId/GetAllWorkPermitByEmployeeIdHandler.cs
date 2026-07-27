using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Get.GetAllByEmployeeId
{
    public sealed class GetAllWorkPermitByEmployeeIdHandler(IWriteContext context) : IRequestHandler<GetAllWorkPermitByEmployeeIdQuery, List<WorkPermitResponse>>
    {
        public async Task<List<WorkPermitResponse>> Handle(GetAllWorkPermitByEmployeeIdQuery request, CancellationToken cancellationToken)
            => await context.WorkPermits
               .Where(x => x.EmployeeId == request.EmployeeId)
               .Select
               (
                   x => new WorkPermitResponse
                   (
                       x.Id.ToString(),
                       request.EmployeeId.ToString(),
                       x.WorkPermitName!,
                       x.DocumentSeries,
                       x.WorkPermitNumber,
                       x.ProtocolNumber,
                       x.SpecialtyName!,
                       x.IssuingAuthority,
                       x.IssueDate,
                       x.ExpiryDate,
                       x.FileKey,
                       x.PermitTypeId.ToString(),
                       x.AdmissionStatusId.ToString()
                   )
               ).ToListAsync(cancellationToken);
    }
}
