using LifeLine.User.Service.Client.Services;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using Shared.WPF.ViewModels.Components;
using System.Windows;

namespace LifeLine.AdminPanel.Desktop.ViewModels.Windows
{
    public sealed class MainWindowVM : BaseWindowViewModel
    {
        private readonly IAuthorizationService _authorizationService;

        public MainWindowVM(IAuthorizationService authorizationService)/* : base(authorizationService)*/
        {
            _authorizationService = authorizationService;

            AuthController = new AuthController(_authorizationService);
        }

        public AuthController AuthController { get; }
    }
}
