using LifeLine.HrPanel.Desktop.Factories.Pages;
using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Services.NavigationService.Pages;

namespace LifeLine.HrPanel.Desktop.Ioc
{
    internal static class RegistrationPage
    {
        public static IServiceCollection UsePage(this ServiceCollection services)
        {
            services.AddTransient<IPageFactory, EmployeePageFactory>();
            services.AddTransient<EmployeePageVM>();
            services.AddSingleton<Func<EmployeePageVM>>(provider => () => provider.GetRequiredService<EmployeePageVM>());

            services.AddTransient<IPageFactory, EmployeeCreatePageFactory>();
            services.AddTransient<EmployeeCreatePageVM>();
            services.AddSingleton<Func<EmployeeCreatePageVM>>(provider => () => provider.GetRequiredService<EmployeeCreatePageVM>());

            services.AddTransient<IPageFactory, EditContactInformationEmployeePageFactory>();
            services.AddTransient<EditContactInformationEmployeePageVM>();
            services.AddSingleton<Func<EditContactInformationEmployeePageVM>>(provider => () => provider.GetRequiredService<EditContactInformationEmployeePageVM>());

            services.AddTransient<IPageFactory, EditPersonalDocumentEmployeePageFactory>();
            services.AddTransient<EditPersonalDocumentEmployeePageVM>();
            services.AddSingleton<Func<EditPersonalDocumentEmployeePageVM>>(provider => () => provider.GetRequiredService<EditPersonalDocumentEmployeePageVM>());

            return services;
        }
    }
}
