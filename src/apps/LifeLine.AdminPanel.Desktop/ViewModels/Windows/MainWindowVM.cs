using LifeLine.User.Service.Client.Services;
using Shared.WPF.Services.NavigationService.Windows;
using Shared.WPF.ViewModels.Abstract;
using Shared.WPF.ViewModels.Components;

namespace LifeLine.AdminPanel.Desktop.ViewModels.Windows
{
    public sealed class MainWindowVM : BaseWindowViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public MainWindowVM(INavigationWindow navigationWindow, IAuthorizationService authorizationService) : base(navigationWindow)
        {
            _authorizationService = authorizationService;

            AuthController = new AuthController(_authorizationService);

            AuthController.AuthCommandAsync!.CanExecuteChanged += (s, e) =>
            {
                MinimizeWindowCommand?.RaiseCanExecuteChanged();
                MaximizeWindowCommand?.RaiseCanExecuteChanged();
                RestoreWindowCommand?.RaiseCanExecuteChanged();
            };
        }

        public AuthController AuthController { get; }
    }
}
