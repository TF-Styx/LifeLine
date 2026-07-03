using LifeLine.Employee.Service.Application.Ico;
using LifeLine.Employee.Service.Infrastructure.Ioc;
using MediatR;
using Shared.Application.Behaviours;
using Shared.Logging;
using Shared.Serialization.Extensions;
using System.Text.Json;

namespace LifeLine.Employee.Service.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.UseInfrastructure(builder.Configuration)/*.UseApplication()*/;
            builder.Services.UseApplication();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DiApplication).Assembly));
            
            builder.Services.Configure<JsonSerializerOptions>(opt => opt.AddTerminexDefault());
            builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.AddTerminexDefault());

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
            builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = builder.Configuration["AutoMapper:AutoMapperKey"], typeof(DiApplication).Assembly);
            builder.Host.UseSerialogLogger();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
