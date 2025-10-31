using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.Gender
{
    public sealed class GenderService(HttpClient httpClient) : BaseHttpService<GenderResponse, string>(httpClient, "api/genders"), IGenderService;
}
