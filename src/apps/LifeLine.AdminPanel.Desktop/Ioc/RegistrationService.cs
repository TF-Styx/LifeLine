using LifeLine.Directory.Service.Client.Services.Department;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;

namespace LifeLine.AdminPanel.Desktop.Ioc
{
    internal static class RegistrationService
    {
        public static IServiceCollection UseAdminPanelServices(this ServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<INavigationPage, NavigationPage>();
            services.AddSingleton<INavigationWindow, NavigationWindow>();

            var directoryService = configuration.GetValue<string>("DirectoryService");
            string directoryHttp = "DirectoryServiceHttp";
            services.AddHttpClient(directoryHttp, client => client.BaseAddress = new Uri(directoryService!));

            services.AddHttpClient<IDepartmentService, DepartmentService>(directoryHttp);

            return services;
        }
    }
}
