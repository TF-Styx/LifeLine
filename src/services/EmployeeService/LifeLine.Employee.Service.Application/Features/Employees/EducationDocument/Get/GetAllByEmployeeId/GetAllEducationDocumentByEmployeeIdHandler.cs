using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Get.GetAllByEmployeeId
{
    public sealed class GetAllEducationDocumentByEmployeeIdHandler(IWriteContext context) 
        : IRequestHandler<GetAllEducationDocumentByEmployeeIdQuery, List<EducationDocumentResponse>>
    {
        public async Task<List<EducationDocumentResponse>> Handle(GetAllEducationDocumentByEmployeeIdQuery request, CancellationToken cancellationToken)
            => await context.EducationDocuments
               .Where(x => x.EmployeeId == request.EmployeeId)
               .Select
               (
                   e => new EducationDocumentResponse
                   (
                       e.Id.ToString(),
                       request.EmployeeId.ToString(),
                       e.EducationLevelId.ToString(),
                       e.DocumentTypeId.ToString(),
                       e.DocumentNumber,
                       e.IssuedDate.ToString(),
                       e.OrganizationName,
                       e.QualificationAwardedName,
                       e.SpecialtyName,
                       e.ProgramName,
                       e.TotalHours.ToString(),
                       e.FileKey
                   )
               ).ToListAsync(cancellationToken);
    }
}
