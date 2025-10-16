using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;

namespace LifeLine.AdminPanel.Desktop.Ioc
{
    internal static class RegistrationService
    {
        public static IServiceCollection UseAdminPanelServices(this ServiceCollection services)
        {
            services.AddSingleton<INavigationPage, NavigationPage>();
            services.AddSingleton<INavigationWindow, NavigationWindow>();

            return services;
        }
    }
}
