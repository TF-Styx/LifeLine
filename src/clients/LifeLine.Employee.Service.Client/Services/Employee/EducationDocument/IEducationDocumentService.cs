using Shared.Contracts.Request.EmployeeService.EducationDocument;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.EducationDocument
{
    public interface IEducationDocumentService : IBaseHttpService<EducationDocumentResponse, string>
    {
        Task<Result> CreateManyAsync(CreateManyEducationDocumentsReqeust reqeust);
        Task<Result> UpdateEducationDocumentAsync(Guid educationDocumentId, UpdateEducationDocumentRequest request);
        Task<Result> DeleteEducationDocumentAsync(Guid educationDocumentId);
    }
}
