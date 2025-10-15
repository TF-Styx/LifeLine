using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using Shared.WPF.ViewModels.Components;
using System.Windows;

namespace LifeLine.AdminPanel.Desktop.ViewModels.Windows
{
    public sealed class MainWindowVM : BaseWindowViewModel
    {
        public MainWindowVM()
        {
            SetValueCommand();
        }

        private void SetValueCommand()
        {
            AuthCommandAsync = new RelayCommandAsync(Execute_AuthCommandAsync, CanExecute_AuthCommandAsync);
        }

        #region Event
        public event Action? ResizeWindow;
        #endregion

        #region Visibility
        private Visibility _authUcVisibility = Visibility.Visible;
        public Visibility AuthVisibility
        {
            get => _authUcVisibility;
            set => SetProperty(ref _authUcVisibility, value);
        }
        #endregion


        public AuthController AuthController { get; } = new ();

        public RelayCommandAsync? AuthCommandAsync {  get; private set; }
        private async Task Execute_AuthCommandAsync()
        {
            AuthVisibility = Visibility.Collapsed;
            ResizeWindow?.Invoke();
            MessageBox.Show("Вы вошли!");
        }
        private bool CanExecute_AuthCommandAsync() 
            => !string.IsNullOrWhiteSpace(AuthController.Login) && 
               !string.IsNullOrWhiteSpace(AuthController.Password);
    }
}
