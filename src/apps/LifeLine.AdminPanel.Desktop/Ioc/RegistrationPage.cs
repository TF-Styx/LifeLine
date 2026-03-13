using LifeLine.AdminPanel.Desktop.Factories.Pages;
using LifeLine.AdminPanel.Desktop.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Services.NavigationService.Pages;

namespace LifeLine.AdminPanel.Desktop.Ioc
{
    internal static class RegistrationPage
    {
        public static IServiceCollection UsePage(this ServiceCollection services)
        {
            services.AddTransient<IPageFactory, EditDepartmentPageFactory>();
            services.AddTransient<EditDepartmentPageVM>();
            services.AddSingleton<Func<EditDepartmentPageVM>>(provider => () => provider.GetRequiredService<EditDepartmentPageVM>());

            return services;
        }
    }
}
