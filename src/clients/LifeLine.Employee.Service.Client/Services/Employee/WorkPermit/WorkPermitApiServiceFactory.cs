namespace LifeLine.Employee.Service.Client.Services.Employee.WorkPermit
{
    public sealed class WorkPermitApiServiceFactory(IHttpClientFactory httpClientFactory) : IWorkPermitApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "EmployeeServiceHttp";

        public IWorkPermitService Create(string employeeId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new WorkPermitService(httpClient, employeeId);
        }
    }
}
