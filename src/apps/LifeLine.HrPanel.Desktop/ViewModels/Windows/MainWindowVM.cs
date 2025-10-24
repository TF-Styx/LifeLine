using LifeLine.User.Service.Client.Services;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;
using Shared.WPF.ViewModels.Abstract;
using Shared.WPF.ViewModels.Components;

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

            AuthController.AuthCommandAsync!.CanExecuteChanged += (s, e) =>
            {
                MinimizeWindowCommand?.RaiseCanExecuteChanged();
                MaximizeWindowCommand?.RaiseCanExecuteChanged();
                RestoreWindowCommand?.RaiseCanExecuteChanged();
            };
        }

        public AuthController AuthController { get; }
        public PopupController RecruitingPopup { get; }
        public PopupController ManagementPopup { get; }
    }
}
