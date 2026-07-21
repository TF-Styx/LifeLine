using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.AssignmentContract;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.EducationDocument;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.Employee;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.WorkPermit;
using Microsoft.Extensions.DependencyInjection;

namespace LifeLine.HrPanel.Desktop.Ioc
{
    internal static class RegistrationEmployeeManagement
    {
        public static IServiceCollection UseEmployeeManagement(this IServiceCollection services)
        {
            services.AddSingleton<ManagementEmployeeStateService>();

            services.AddTransient<EmployeeListVM>();
            services.AddTransient<EmployeeEditVM>();
            services.AddTransient<EmployeeManagementVM>();
            services.AddTransient<Func<EmployeeManagementVM>>(provider => () => provider.GetRequiredService<EmployeeManagementVM>());

            services.AddTransient<PersonalDocumentListVM>();
            services.AddTransient<PersonalDocumentEditVM>();
            services.AddTransient<PersonalDocumentManagementVM>();
            services.AddTransient<Func<PersonalDocumentManagementVM>>(provider => () => provider.GetRequiredService<PersonalDocumentManagementVM>());

            services.AddTransient<EducationDocumentListVM>();
            services.AddTransient<EducationDocumentEditVM>();
            services.AddTransient<EducationDocumentManagementVM>();
            services.AddTransient<Func<EducationDocumentManagementVM>>(provider => () => provider.GetRequiredService<EducationDocumentManagementVM>());

            services.AddTransient<WorkPermitListVM>();
            services.AddTransient<WorkPermitEditVM>();
            services.AddTransient<WorkPermitManagementVM>();
            services.AddTransient<Func<WorkPermitManagementVM>>(provider => () => provider.GetRequiredService<WorkPermitManagementVM>());

            services.AddTransient<AssignmentContractListVM>();
            services.AddTransient<AssignmentContractEditVM>();
            services.AddTransient<AssignmentContractManagementVM>();
            services.AddTransient<Func<AssignmentContractManagementVM>>(provider => () => provider.GetRequiredService<AssignmentContractManagementVM>());

            return services;
        }
    }
}
