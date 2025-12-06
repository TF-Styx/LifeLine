using Shared.Contracts.Request.EmployeeService.Employee;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee
{
    public interface IEmployeeService : IBaseHttpService<EmployeeResponse, string>
    {
        Task<List<EmployeeHrItemResponse>> GetAllForHrAsync();
        Task<EmployeeFullDetailsResponse?> GetDetailsAsync(string id);
        Task<Result> UpdateEmployeeAsync(string employeeId, UpdateEmployeeRequest request);
    }
}
