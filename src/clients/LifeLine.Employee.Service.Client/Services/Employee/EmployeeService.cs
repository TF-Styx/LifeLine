using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.Employee
{
    public sealed class EmployeeService(HttpClient httpClient) : BaseHttpService<EmployeeResponse, string>(httpClient, "api/employees"), IEmployeeService
    {

    }
}
