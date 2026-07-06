using Shared.Contracts.Request.EmployeeService.PersonalDocument;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument
{
    public interface IPersonalDocumentService : IBaseHttpService<PersonalDocumentResponse, string>
    {
        Task<Result> CreateManyAsync(CreateManyPersonalDocumentsRequest request);
        Task<Result<List<PersonalDocumentResponse>>> GetAllByEmployeeId(string employeeId);
        Task<Result> UpdatePersonalDocumentAsync(Guid personalDocumentId, UpdatePersonalDocumentRequest request);
        Task<Result> DeletePersonalDocumentAsync(Guid personalDocumentId);
    }
}
