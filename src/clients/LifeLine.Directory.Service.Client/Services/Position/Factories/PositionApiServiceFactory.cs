namespace LifeLine.Directory.Service.Client.Services.Position.Factories
{
    public sealed class PositionApiServiceFactory(IHttpClientFactory httpClientFactory) : IPositionApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "DirectoryApiClient";

        public IPositionService Create(string departmentId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new PositionService(httpClient, departmentId);
        }
    }
}
