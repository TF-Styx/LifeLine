using Microsoft.Extensions.DependencyInjection;

namespace LifeLine.AdminPanel.Desktop.Ioc
{
    internal static class RegistrationPage
    {
        public static IServiceCollection UsePage(this ServiceCollection services)
        {
            return services;
        }
    }
}
