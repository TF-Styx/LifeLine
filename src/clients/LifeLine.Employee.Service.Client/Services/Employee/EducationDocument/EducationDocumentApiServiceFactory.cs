using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee.EducationDocument
{
    public sealed class EducationDocumentApiServiceFactory(IHttpClientFactory httpClientFactory, IOptions<JsonSerializerOptions> options) : IEducationDocumentApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "EmployeeServiceHttp";

        public IEducationDocumentService Create(string employeeId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new EducationDocumentService(httpClient, employeeId, options);
        }
    }
}
