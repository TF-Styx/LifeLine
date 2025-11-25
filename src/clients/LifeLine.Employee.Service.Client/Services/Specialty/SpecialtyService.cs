using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.Specialty
{
    public class SpecialtyService(HttpClient httpClient) : BaseHttpService<SpecialtyResponse, string>(httpClient, "api/specialties"), ISpecialtyService;
}
