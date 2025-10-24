using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;

namespace LifeLine.HrPanel.Desktop.Ioc
{
    internal static class RegistrationService
    {
        public static IServiceCollection UseHrPanelServices(this ServiceCollection services)
        {
            services.AddSingleton<INavigationPage, NavigationPage>();
            services.AddSingleton<INavigationWindow, NavigationWindow>();

            return services;
        }
    }
}
