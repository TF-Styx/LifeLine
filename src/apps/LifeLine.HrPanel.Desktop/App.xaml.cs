using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Ioc;
using LifeLine.HrPanel.Desktop.Services.App;
using LifeLine.HrPanel.Desktop.Services.Secure;
using LifeLine.HrPanel.Desktop.ViewModels.Windows;
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

            SetUpAuthenticationHandler();

            // ServiceProvider.GetService<INavigationWindow>()!.OpenWindow(WindowName.MainWindow);

            var init = ServiceProvider.GetService<IInitializationService>();

            try
            {
                init!.Initialization();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка!\n{ex}");
                Shutdown();
            }

            base.OnStartup(e);
        }

        private void SetUpAuthenticationHandler()
        {
            var authenticationStateService = ServiceProvider.GetRequiredService<IAuthenticationStateService>();
            var navigationWindow = ServiceProvider.GetRequiredService<INavigationWindow>();

            authenticationStateService.AuthenticationRequired += () =>
            {
                var mainWindow = navigationWindow.GetWindow(WindowName.MainWindow);

                if (mainWindow == null)
                    return;

                if (mainWindow.DataContext is not MainWindowVM mainWindowVM)
                    return;

                mainWindowVM.AuthController.AuthVisibility = mainWindow.Visibility;
                mainWindowVM.AuthController.ExecuteResizeWindowAfterLogout();
            };
        }
    }
}
