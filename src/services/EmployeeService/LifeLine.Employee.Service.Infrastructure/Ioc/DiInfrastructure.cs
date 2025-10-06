using LifeLine.Employee.Service.Infrastructure.Persistence.Contexts;
using LifeLine.Employee.Service.Infrastructure.Persistence.Repository;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeLine.Employee.Service.Infrastructure.Ioc
{
    public static class DiInfrastructure
    {
        public static IServiceCollection UseInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<EmployeeWriteContext>(option => option.UseNpgsql(connectionString));
            services.AddScoped<IWriteContext>(provider => provider.GetRequiredService<EmployeeWriteContext>());

            services.AddDbContext<EmployeeReadContext>(option =>
            {
                option.UseNpgsql(connectionString);
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<IReadContext>(provider => provider.GetRequiredService<EmployeeReadContext>());

            services.AddScoped<IGenderRepository, GenderRepository>();

            return services;
        }
    }
}
