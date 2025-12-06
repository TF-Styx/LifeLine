namespace LifeLine.Employee.Service.Client.Services.Employee.ContactInformation
{
    public sealed class ContactInformationApiServiceFactory(IHttpClientFactory httpClientFactory) : IContactInformationApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "EmployeeServiceHttp";

        public IContactInformationService Create(string employeeId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new ContactInformationService(httpClient, employeeId);
        }
    }
}
