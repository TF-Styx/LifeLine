using LifeLine.AdminPanel.Desktop.ViewModels.Windows;
using LifeLine.AdminPanel.Desktop.Views.Windows;
using Shared.WPF.Enums;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;
using System.Windows;
using System.Windows.Controls;

namespace LifeLine.AdminPanel.Desktop.Factories.Windows
{
    internal sealed class MainWindowFactory(Func<MainWindowVM> viewModelFactory, INavigationPage navigationPage) : IWindowFactory
    {
        private readonly Func<MainWindowVM> _viewModelFactory = viewModelFactory;
        private readonly INavigationPage _navigationPage = navigationPage;

        public Window Create()
        {
            var vm = _viewModelFactory();
            var window = new MainWindow(vm);

            window.Loaded += (sender, args) =>
            {
                var frame = window.FindName("MainFrame") as Frame;
                _navigationPage.SetFrame(FrameName.MainFrame, frame);
            };

            return window;
        }
    }
}
