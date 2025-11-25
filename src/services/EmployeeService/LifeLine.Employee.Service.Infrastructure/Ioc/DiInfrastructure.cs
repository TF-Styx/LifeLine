using LifeLine.Employee.Service.Infrastructure.Persistence.Contexts;
using LifeLine.Employee.Service.Infrastructure.Persistence.Repository;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using LifeLine.EmployeeService.Application.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace LifeLine.Employee.Service.Infrastructure.Ioc
{
    public static class DiInfrastructure
    {
        public static IServiceCollection UseInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            dataSourceBuilder.EnableDynamicJson(
                jsonbClrTypes:
                [
                    typeof(GenderDetailsViewData),
                    typeof(ContactInformationDetailsViewData),
        
                    typeof(List<AssignmentDetailsViewData>),
                    typeof(List<ContractDetailsViewData>),
                    typeof(List<EducationDocumentDetailsViewData>),
                    typeof(List<PersonalDocumentDetailsViewDate>),
                    typeof(List<SpecialtyDetailsViewData>),
                    typeof(List<WorkPermitDetailsViewData>)
                ]
            );

            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<EmployeeWriteContext>(option => option.UseNpgsql(connectionString));
            services.AddScoped<IWriteContext>(provider => provider.GetRequiredService<EmployeeWriteContext>());

            services.AddDbContext<EmployeeReadContext>(option =>
            {
                option.UseNpgsql(dataSource);
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped<IReadContext>(provider => provider.GetRequiredService<EmployeeReadContext>());

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();

            return services;
        }
    }
}
