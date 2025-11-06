namespace LifeLine.Directory.Service.Client.Services.Position.Factories
{
    public sealed class PositionReadOnlyApiServiceFactory(IHttpClientFactory httpClientFactory) : IPositionReadOnlyApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "DirectoryServiceHttp";

        public IPositionReadOnlyService Create(string departmentId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new PositionService(httpClient, departmentId);
        }
    }
}
