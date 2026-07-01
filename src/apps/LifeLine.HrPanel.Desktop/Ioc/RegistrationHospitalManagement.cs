using Microsoft.Extensions.DependencyInjection;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Branch;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Hospital;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Department;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Position;

namespace LifeLine.HrPanel.Desktop.Ioc
{
    internal static class RegistrationHospitalManagement
    {
        public static IServiceCollection UseHospitalManagement(this IServiceCollection services)
        {
            services.AddSingleton<ManagementHospitalStateService>();

            services.AddTransient<HospitalListVM>();
            services.AddTransient<HospitalEditVM>();
            services.AddTransient<HospitalManagementVM>();
            services.AddTransient<Func<HospitalManagementVM>>(provider => () => provider.GetRequiredService<HospitalManagementVM>());

            services.AddTransient<BranchListVM>();
            services.AddTransient<BranchEditVM>();
            services.AddTransient<BranchManagementVM>();
            services.AddTransient<Func<BranchManagementVM>>(provider => () => provider.GetRequiredService<BranchManagementVM>());

            services.AddTransient<DepartmentListVM>();
            services.AddTransient<DepartmentEditVM>();
            services.AddTransient<DepartmentManagementVM>();
            services.AddTransient<Func<DepartmentManagementVM>>(provider => () => provider.GetRequiredService<DepartmentManagementVM>());

            services.AddTransient<PositionListVM>();
            services.AddTransient<PositionEditVM>();
            services.AddTransient<PositionManagementVM>();
            services.AddTransient<Func<PositionManagementVM>>(provider => () => provider.GetRequiredService<PositionManagementVM>());

            return services;
        }
    }
}
