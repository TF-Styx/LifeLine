using LifeLine.User.Service.Client.Services;
using Shared.WPF.Commands;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;
using Shared.WPF.ViewModels.Abstract;
using Shared.WPF.ViewModels.Components;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Windows
{
    public sealed class MainWindowVM : BaseWindowViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public MainWindowVM
            (
                INavigationWindow navigationWindow,
                INavigationPage navigationPage,
                IAuthorizationService authorizationService
            ) : base(navigationWindow, navigationPage)
        {
            _authorizationService = authorizationService;

            AuthController = new AuthController(_authorizationService);
            RecruitingPopup = new PopupController();
            ManagementPopup = new PopupController();
            ProfilePopup = new PopupController();

            AuthController.AuthCommandAsync!.CanExecuteChanged += (s, e) =>
            {
                MinimizeWindowCommand?.RaiseCanExecuteChanged();
                MaximizeWindowCommand?.RaiseCanExecuteChanged();
                RestoreWindowCommand?.RaiseCanExecuteChanged();
            };

            AuthController.ResizeWindowAfterLogin += () => IsLoggedIn = true;
            AuthController.ResizeWindowAfterLogout += () => IsLoggedIn = false;

            LogoutCommand = new RelayCommandAsync(Execute_LogoutAsync);
        }

        public bool IsLoggedIn
        { 
            get => field;
            set => SetProperty(ref field, value);
        }

        public AuthController AuthController { get; }
        public PopupController RecruitingPopup { get; }
        public PopupController ManagementPopup { get; }
        public PopupController ProfilePopup { get; }

        public RelayCommandAsync LogoutCommand { get; private init; }
        private async Task Execute_LogoutAsync()
        {
            IsLoggedIn = false;

            await _authorizationService.LogoutAsync();

            AuthController.AuthVisibility = Visibility.Visible;
            AuthController.ExecuteResizeWindowAfterLogout();
        }
    }
}
