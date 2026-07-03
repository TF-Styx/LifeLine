using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument
{
    public sealed class PersonalDocumentApiServiceFactory(IHttpClientFactory httpClientFactory, IOptions<JsonSerializerOptions> options) : IPersonalDocumentApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "EmployeeServiceHttp";

        public IPersonalDocumentService Create(string employeeId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new PersonalDocumentService(httpClient, employeeId, options);
        }
    }
}
