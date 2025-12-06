using Shared.Contracts.Request.EmployeeService.ContactInformation;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.ContactInformation
{
    public interface IContactInformationService : IBaseHttpService<ContactInformationResponse, string>
    {
        Task<Result> UpdateContactInformationAsync(UpdateContactInformationRequest request);
    }
}
