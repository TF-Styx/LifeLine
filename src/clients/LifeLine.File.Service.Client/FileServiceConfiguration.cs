using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeLine.File.Service.Client
{
    public static class FileServiceConfiguration
    {
        public static IServiceCollection UseFileService(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("FileService");

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException($"Строка подключения была пустой!");

            const string fileUrl = "FileUrl";

            services.AddHttpClient(fileUrl, client => client.BaseAddress = new Uri(url));

            services.AddHttpClient<IFileStorageService, FileStorageService>(fileUrl);

            return services;
        }
    }
}
