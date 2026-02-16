using LifeLine.User.Service.Client.ApiClients;
using LifeLine.User.Service.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Client.Security.Abstraction;
using Shared.Client.Security.SRP;
using Shared.Client.Security.Windows;

namespace LifeLine.User.Service.Client.Ioc
{
    public static class UserServiceConfiguration
    {
        public static IServiceCollection UserClientConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            string? userServiceApiUrl = configuration.GetValue<string>("UserService");

            if (string.IsNullOrWhiteSpace(userServiceApiUrl))
                throw new Exception("В файле конфигурации, нет такой строки!");

            const string userApiName = "UserApi";

            services.AddHttpClient(userApiName, client => client.BaseAddress = new Uri(userServiceApiUrl));

            services.AddHttpClient<IUserApiService, UserApiService>(userApiName);
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
            services.AddSingleton<ISRPService, SRPService>();
            services.AddSingleton<ITokenStorage, TokenStorage>();

            return services;
        }
    }
}
