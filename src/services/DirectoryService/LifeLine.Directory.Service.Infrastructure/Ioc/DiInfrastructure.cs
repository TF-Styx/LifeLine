using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Infrastructure.Persistence.Contexts;
using LifeLine.Directory.Service.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeLine.Directory.Service.Infrastructure.Ioc
{
    public static class DiInfrastructure
    {
        public static IServiceCollection UseInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DirectoryContext>(option => option.UseNpgsql(connectionString));
            services.AddScoped<IDirectoryContext>(provider => provider.GetRequiredService<DirectoryContext>());

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IEducationLevelRepository, EducationLevelRepository>();
            services.AddScoped<IAdmissionStatusRepository, AdmissionStatusRepository>();
            services.AddScoped<IPermitTypeRepository, PermitTypeRepository>();

            return services;
        }
    }
}
