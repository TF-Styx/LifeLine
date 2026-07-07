using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Infrastructure.Ioc;
using LifeLine.Employee.Service.Client.Services.Employee.Assignment;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Application.Behaviours;
using Shared.Logging;
using Shared.Serialization.Extensions;
using System.Text;
using System.Text.Json;

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

            builder.Services.Configure<JsonSerializerOptions>(opt => opt.AddTerminexDefault());
            builder.Services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.AddTerminexDefault());

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
            builder.Services.AddHttpClient<IAssignmentCheckService, AssignmentCheckService>(client => client.BaseAddress = new Uri(builder.Configuration["EmployeeService"]!));
            builder.Host.UseSerialogLogger();

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer
            (
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters 
                    { 
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                }
            );

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
