using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Get.GetAllByEmployeeId
{
    public sealed class GetAllPersonalDocumentByEmployeeIdHandler(IWriteContext context) 
        : IRequestHandler<GetAllPersonalDocumentByEmployeeIdQuery, List<PersonalDocumentResponse>>
    {
        public async Task<List<PersonalDocumentResponse>> Handle(GetAllPersonalDocumentByEmployeeIdQuery request, CancellationToken cancellationToken)
            => await context.PersonalDocuments
               .Where(x => x.EmployeeId == request.EmployeeId)
               .Select
               (
                   x => new PersonalDocumentResponse
                   (
                       x.Id,
                       x.DocumentTypeId,
                       x.DocumentNumber,
                       x.DocumentSeries,
                       x.ImageKey
                   )
               ).ToListAsync(cancellationToken);
    }
}
