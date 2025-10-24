using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.Services.NavigationService.Windows;
using System.Windows;

namespace Shared.WPF.ViewModels.Abstract
{
    public abstract class BaseWindowViewModel : BaseViewModel
    {
        public readonly INavigationWindow _navigationWindow;
        public readonly INavigationPage _navigationPage;

        protected BaseWindowViewModel(INavigationWindow navigationWindow, INavigationPage navigationPage)
        {
            _navigationWindow = navigationWindow;
            _navigationPage = navigationPage;

            SetValueCommands();
        }

        private void SetValueCommands()
        {
            OpenPageCommand = new RelayCommand<PageName>(Execute_OpenPageCommand);

            ShutDownAppCommand = new RelayCommand(Execute_ShutDownAppCommand);

            MinimizeWindowCommand = new RelayCommand<WindowName>(Execute_MinimizeWindowCommand, CanExecute_MinimizeWindowCommand);
            MaximizeWindowCommand = new RelayCommand<WindowName>(Execute_MaximizeWindowCommand, CanExecute_MaximizeWindowCommand);
            RestoreWindowCommand = new RelayCommand<WindowName>(Execute_RestoreWindowCommand, CanExecute_RestoreWindowCommand);
            CloseWindowCommand = new RelayCommand<WindowName>(Execute_CloseWindowCommand);
        }

        private string _windowTitle = null!;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        #region Открытия страницы

        public RelayCommand<PageName>? OpenPageCommand { get; private set; }

        private void Execute_OpenPageCommand(PageName page) => _navigationPage?.NavigateTo(FrameName.MainFrame, page);

        #endregion

        #region ShutDownApp

        public RelayCommand? ShutDownAppCommand { get; private set; }

        private void Execute_ShutDownAppCommand() => System.Windows.Application.Current.Shutdown();

        #endregion

        #region Minimize

        public RelayCommand<WindowName>? MinimizeWindowCommand { get; private set; }

        private void Execute_MinimizeWindowCommand(WindowName windowName) => _navigationWindow!.MinimizeWindow(windowName);
        private bool CanExecute_MinimizeWindowCommand(WindowName windowName)
            => _navigationWindow.GetCurrentResizeMode(windowName) != ResizeMode.NoResize;

        #endregion

        #region Maximize

        public RelayCommand<WindowName>? MaximizeWindowCommand { get; private set; }

        private void Execute_MaximizeWindowCommand(WindowName windowName) => _navigationWindow!.MaximizeWindow(windowName);
        private bool CanExecute_MaximizeWindowCommand(WindowName windowName)
            => _navigationWindow.GetCurrentResizeMode(windowName) != ResizeMode.NoResize;

        #endregion

        #region Restore

        public RelayCommand<WindowName>? RestoreWindowCommand { get; private set; }

        private void Execute_RestoreWindowCommand(WindowName windowName) => _navigationWindow!.RestoreWindow(windowName);
        private bool CanExecute_RestoreWindowCommand(WindowName windowName)
            => _navigationWindow.GetCurrentResizeMode(windowName) != ResizeMode.NoResize;

        #endregion

        #region Close

        public RelayCommand<WindowName>? CloseWindowCommand { get; private set; }

        private void Execute_CloseWindowCommand(WindowName windowName) => _navigationWindow!.Close(windowName);

        #endregion
    }
}
