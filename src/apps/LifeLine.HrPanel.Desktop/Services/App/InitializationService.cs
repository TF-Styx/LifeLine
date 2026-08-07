using Shared.Client.Security.Abstraction;
using LifeLine.User.Service.Client.ApiClients;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;
using Shared.WPF.Enums;
using Shared.Contracts.Request.UserService;
using LifeLine.HrPanel.Desktop.ViewModels.Windows;
using System.Windows;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;

namespace LifeLine.HrPanel.Desktop.Services.App
{
    public sealed class InitializationService
            (
                ITokenStorage tokenStorage,
                IKeyManager keyManager,
                IUserApiService authService,
                INavigationPage navigationPage,
                INavigationWindow navigationWindow,
                IReferenceDataCacheService cacheService,
                IAssignmentCascadeService cascadeService
            ) : IInitializationService
    {
        public async Task Initialization()
        {
            var mainWindow = navigationWindow.CreateMainWindowWithoutOpen();

            if (mainWindow == null)
            {
                MessageBox.Show("Окно не открылось!");
                return;
            }

            var mainWindowVM = mainWindow.DataContext as MainWindowVM;

            var accessToken = await tokenStorage.GetAccessTokenAsync();
            var refreshToken = await tokenStorage.GetRefrashTokenAsync();

            if (string.IsNullOrWhiteSpace(refreshToken) && string.IsNullOrWhiteSpace(accessToken))
            {
                navigationWindow.OpenWindow(WindowName.MainWindow);
                return;
            }

            var result = await authService.RefreshToken(new LoginByTokenRequest(refreshToken!, accessToken));

            if (result.IsFailure)
            {
                navigationWindow.OpenWindow(WindowName.MainWindow);
                return;
            }

            await tokenStorage.SaveAsync(result.Value.AccessToken, result.Value.RefreshToken);
            await Task.WhenAll
            (
                cacheService.InitializeAsync(),
                cascadeService.InitializeAsync()
            );

            mainWindowVM.AuthController.AuthVisibility = Visibility.Collapsed;
            mainWindowVM.AuthController.ExecuteResizeWindowAfterLogin();
            mainWindow.Show();
        }
    }
}