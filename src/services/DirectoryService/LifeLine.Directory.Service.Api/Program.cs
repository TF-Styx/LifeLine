using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Infrastructure.Ioc;
using Shared.Logging;

namespace LifeLine.Directory.Service.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.UseInfrastructure(builder.Configuration);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IDirectoryContext).Assembly));
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
