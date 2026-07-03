using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.Position.Factories
{
    public sealed class PositionApiServiceFactory(IHttpClientFactory httpClientFactory, IOptions<JsonSerializerOptions> options) : IPositionApiServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly string _clientName = "DirectoryServiceHttp";

        public IPositionService Create(string departmentId)
        {
            var httpClient = _httpClientFactory.CreateClient(_clientName);

            return new PositionService(httpClient, departmentId, options);
        }
    }
}
