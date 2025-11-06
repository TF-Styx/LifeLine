using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.EmployeeType
{
    public sealed class EmployeeTypeService(HttpClient httpClient) : BaseHttpService<EmployeeTypeResponse, string>(httpClient, "api/employee-types"), IEmployeeTypeService;
}
