using Microsoft.Extensions.Options;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.Gender
{
    public sealed class GenderService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<GenderResponse, string>(httpClient, "api/genders", options.Value), IGenderService;
}
