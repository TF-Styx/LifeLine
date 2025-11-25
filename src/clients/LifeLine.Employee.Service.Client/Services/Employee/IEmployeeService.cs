using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.Employee
{
    public interface IEmployeeService : IBaseHttpService<EmployeeResponse, string>
    {
        Task<List<EmployeeHrItemResponse>> GetAllForHrAsync();
        Task<EmployeeFullDetailsResponse?> GetDetailsAsync(string id);
    }
}
