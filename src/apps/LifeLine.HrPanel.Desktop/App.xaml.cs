using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Ioc;
using LifeLine.User.Service.Client.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Enums;
using Shared.WPF.Services.NavigationService.Windows;
using System.Windows;

namespace LifeLine.HrPanel.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Находит файл apiConfig со строкой подключения
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("apiConfig.json", optional: false, reloadOnChange: false);

            IConfigurationRoot configuration = configurationBuilder.Build();

            var servicesCollection = new ServiceCollection();

            // Регистрация сервисов
            servicesCollection.UserClientConfiguration(configuration);
            servicesCollection.UseHrPanelServices(configuration);
            servicesCollection.UseWindow();
            servicesCollection.UsePage();
            servicesCollection.UseFileService(configuration);

            ServiceProvider = servicesCollection.BuildServiceProvider();

            ServiceProvider.GetService<INavigationWindow>()!.OpenWindow(WindowName.MainWindow);

            base.OnStartup(e);
        }
    }
}
