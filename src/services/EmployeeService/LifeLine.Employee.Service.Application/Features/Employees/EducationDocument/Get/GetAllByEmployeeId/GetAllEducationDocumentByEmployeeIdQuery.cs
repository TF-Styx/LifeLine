using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Get.GetAllByEmployeeId
{
    public sealed record GetAllEducationDocumentByEmployeeIdQuery(Guid EmployeeId) : IRequest<List<EducationDocumentResponse>>;
}
