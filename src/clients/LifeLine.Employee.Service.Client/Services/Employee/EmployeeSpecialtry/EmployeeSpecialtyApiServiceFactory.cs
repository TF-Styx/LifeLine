using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    public sealed class EmployeeSpecialtyApiServiceFactory(IHttpClientFactory httpClientFactory, IOptions<JsonSerializerOptions> options) : IEmployeeSpecialtyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "EmployeeServiceHttp";

        public IEmployeeSpecialtyService Create(string employeeId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new EmployeeSpecialtyService(httpClient, employeeId, options);
        }
    }
}
