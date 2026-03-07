using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeLine.File.Service.Client
{
    public static class FileServiceConfiguration
    {
        public static IServiceCollection UseFileService(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("Minerva");

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException($"Строка подключения была пустой!");

            const string minervaS3Url = "MinervaUrl";

            services.AddHttpClient(minervaS3Url, client => client.BaseAddress = new Uri(url));

            services.AddHttpClient<IFileStorageService, FileStorageService>(minervaS3Url);

            return services;
        }
    }
}
