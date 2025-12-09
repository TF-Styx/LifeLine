namespace LifeLine.Employee.Service.Client.Services.Employee.EducationDocument
{
    public sealed class EducationDocumentApiServiceFactory(IHttpClientFactory httpClientFactory) : IEducationDocumentApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "EmployeeServiceHttp";

        public IEducationDocumentService Create(string employeeId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new EducationDocumentService(httpClient, employeeId);
        }
    }
}
