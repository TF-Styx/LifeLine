namespace LifeLine.File.Service.Client
{
    internal class FileStorageService(HttpClient httpClient) : IFileStorageService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<string> GetLink(string key)
        {
            var encodeKey = Uri.EscapeDataString(key);

            var response = await _httpClient.GetAsync($"api/files/link?key={encodeKey}");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
