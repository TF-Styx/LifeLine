using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.View.Windows;
using LifeLine.MVVM.ViewModel;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using LifeLine.Services.NavigationPages;
using LifeLine.Services.NavigationWindow;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LifeLine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceCollection services = new ServiceCollection();

            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private static void ConfigureServices(ServiceCollection service)
        {
            service.AddSingleton<MainWindowVM>();
            service.AddSingleton<MainWindow>();

            service.AddSingleton<IDialogService, DialogService>();
            service.AddSingleton<INavigationWindow, NavigationWindow>();
            service.AddSingleton<INavigationPage, NavigationPage>();
            service.AddSingleton<IDataBaseService, DataBaseService>();
            service.AddSingleton<IOpenFileDialogService, OpenFileDialogService>();

            service.AddTransient<EmployeeManagementContext>();
            service.AddSingleton<Func<EmployeeManagementContext>>(provider => () => provider.GetRequiredService<EmployeeManagementContext>());
        }

        //  AddSingleton
        //      Описание: Экземпляр создается один раз и используется на протяжении всего времени работы приложения.Все запросы к сервису будут получать один и тот же экземпляр.
        //      Когда использовать: Для тяжелых или долговечных объектов, которые должны сохранять состояние и инициализируются один раз.
        //  AddTransient - 
        //    Описание: Каждый раз, когда вы запрашиваете сервис, создается новый экземпляр.
        //    Когда использовать: Для легковесных и независимых сервисов, где создание нового объекта не требует значительных ресурсов.
        //  AddScoped
        //      Описание: Экземпляр создается один раз на каждый запрос.Это означает, что в пределах одного HTTP-запроса(например, в ASP.NET Core) вы получите один и тот же экземпляр, но для разных запросов будут создаваться разные экземпляры.
        //      Когда использовать: Когда необходимо, чтобы сервис разделялся между компонентами в рамках одного запроса, но не между разными запросами.
    }

}
