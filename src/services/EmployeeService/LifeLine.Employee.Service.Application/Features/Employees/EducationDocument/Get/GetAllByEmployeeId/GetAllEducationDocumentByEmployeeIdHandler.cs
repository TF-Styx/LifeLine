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
        {
            var employee = await context.Employees.Include(x => x.EducationDocuments)
                .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken);

            return employee!.EducationDocuments.Select
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
                ).ToList();
        }
    }
}
