using MediatR;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Get.GetAllByEmployeeId
{
    public sealed record GetAllPersonalDocumentByEmployeeIdQuery(Guid EmployeeId) : IRequest<List<PersonalDocumentResponse>>;
}
