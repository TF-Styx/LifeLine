using FluentValidation;
using LifeLine.Employee.Service.Application.Features.Genders.Create;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Api.Application.Behaviors;

namespace LifeLine.Employee.Service.Application.Ico
{
    public static class DiApplication
    {
        public static IServiceCollection UseApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateGenderCommandValidator).Assembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
