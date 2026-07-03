using Microsoft.Extensions.Options;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.Specialty
{
    public class SpecialtyService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<SpecialtyResponse, string>(httpClient, "api/specialties", options.Value), ISpecialtyService;
}
