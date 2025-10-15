using Shared.WPF.ViewModels.Abstract;

namespace Shared.WPF.ViewModels.Components
{
    public sealed class AuthController : BaseViewModel
    {
        private string _login = null!;
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        private string _password = null!;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
    }
}
