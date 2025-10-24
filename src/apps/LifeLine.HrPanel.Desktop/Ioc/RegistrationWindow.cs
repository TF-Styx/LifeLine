using LifeLine.HrPanel.Desktop.Factories.Windows;
using LifeLine.HrPanel.Desktop.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Services.NavigationService.Windows;

namespace LifeLine.HrPanel.Desktop.Ioc
{
    internal static class RegistrationWindow
    {
        public static IServiceCollection UseWindow(this ServiceCollection services)
        {
            services.AddTransient<IWindowFactory, MainWindowFactory>();
            services.AddTransient<MainWindowVM>();
            services.AddSingleton<Func<MainWindowVM>>(provider => () => provider.GetRequiredService<MainWindowVM>());

            return services;
        }
    }
}
