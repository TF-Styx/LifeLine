using Microsoft.Extensions.Options;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.EmployeeType
{
    public sealed class EmployeeTypeService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<EmployeeTypeResponse, string>(httpClient, "api/employee-types", options.Value), IEmployeeTypeService;
}
