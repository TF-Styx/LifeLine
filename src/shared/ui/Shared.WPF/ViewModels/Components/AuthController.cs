using LifeLine.User.Service.Client.Models.Request;
using LifeLine.User.Service.Client.Services;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace Shared.WPF.ViewModels.Components
{
    public sealed class AuthController : BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;

            AuthCommandAsync = new RelayCommandAsync(Execute_AuthCommandAsync, CanExecute_AuthCommandAsync);
        }

        public event Action? ResizeWindow;

        private Visibility _authUcVisibility = Visibility.Visible;
        public Visibility AuthVisibility
        {
            get => _authUcVisibility;
            set => SetProperty(ref _authUcVisibility, value);
        }

        private string _login = "Styx";
        public string Login
        {
            get => _login;
            set
            {
                SetProperty(ref _login, value);
                AuthCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string _password = "Csgofast567!";
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                AuthCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        public RelayCommandAsync? AuthCommandAsync { get; private set; }
        private async Task Execute_AuthCommandAsync()
        {
            var result = await _authorizationService.AuthAsync(new UserRequest(Login, Password));

            if (result.IsSuccess)
            {
                AuthVisibility = Visibility.Collapsed;
                ResizeWindow?.Invoke();

                return;
            }

            MessageBox.Show(result.StringMessage);
        }
        private bool CanExecute_AuthCommandAsync()
            => !string.IsNullOrWhiteSpace(Login) &&
               !string.IsNullOrWhiteSpace(Password);
    }
}
