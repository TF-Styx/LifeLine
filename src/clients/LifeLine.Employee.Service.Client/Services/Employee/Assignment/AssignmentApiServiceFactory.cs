using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee.Assignment
{
    public sealed class AssignmentApiServiceFactory(IHttpClientFactory httpClientFactory, IOptions<JsonSerializerOptions> options) : IAssignmentApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "EmployeeServiceHttp";

        public IAssignmentService Create(string employeeId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new AssignmentService(httpClient, employeeId, options);
        }
    }
}
