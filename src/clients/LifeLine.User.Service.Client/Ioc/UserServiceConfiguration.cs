using LifeLine.User.Service.Client.ApiClients;
using LifeLine.User.Service.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeLine.User.Service.Client.Ioc
{
    public static class UserServiceConfiguration
    {
        public static IServiceCollection UserClientConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            string? userServiceApiUrl = configuration.GetValue<string>("UserServiceUrl");

            if (string.IsNullOrWhiteSpace(userServiceApiUrl))
                throw new Exception("В файле конфигурации, нет такой строки!");

            const string userApiName = "UserApi";

            services.AddHttpClient(userApiName, client => client.BaseAddress = new Uri(userServiceApiUrl));

            services.AddHttpClient<IUserApiService, UserApiService>(userApiName);
            services.AddSingleton<IAuthorizationService, AuthorizationService>();

            return services;
        }
    }
}
