using Shared.Contracts.Request.EmployeeService.EducationDocument;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.EducationDocument
{
    public interface IEducationDocumentService : IBaseHttpService<EducationDocumentResponse, string>
    {
        Task<Result> UpdateEducationDocumentAsync(Guid educationDocumentId, UpdateEducationDocumentRequest request);
    }
}
