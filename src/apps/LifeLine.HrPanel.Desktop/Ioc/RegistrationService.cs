using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Directory.Service.Client.Services.EducationLevel;
using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.Employee.Service.Client.Services.Gender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;

namespace LifeLine.HrPanel.Desktop.Ioc
{
    internal static class RegistrationService
    {
        public static IServiceCollection UseHrPanelServices(this ServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<INavigationPage, NavigationPage>();
            services.AddSingleton<INavigationWindow, NavigationWindow>();

            var employeeService = configuration.GetValue<string>("EmployeeService");
            string employeeHttp = "EmployeeServiceHttp";
            services.AddHttpClient(employeeHttp, client => client.BaseAddress = new Uri(employeeService!));
            services.AddHttpClient<IEmployeeService, EmployeeService>(employeeHttp);
            services.AddHttpClient<IGenderReadOnlyService, GenderService>(employeeHttp);

            var directoryService = configuration.GetValue<string>("DirectoryService");
            string directoryHttp = "DirectoryServiceHttp";
            services.AddHttpClient(directoryHttp, client => client.BaseAddress = new Uri(directoryService!));
            services.AddHttpClient<IDocumentTypeReadOnlyService, DocumentTypeService>(directoryHttp);
            services.AddHttpClient<IEducationLevelReadOnlyService, EducationLevelService>(directoryHttp);

            return services;
        }
    }
}
