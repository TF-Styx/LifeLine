using Shared.WPF.Enums;

namespace Shared.WPF.Services.NavigationService.Windows
{
    public interface INavigationWindow
    {
        void Close(WindowName windowName);
        void MaximizeWindow(WindowName windowName);
        void MinimizeWindow(WindowName windowName);
        void OpenWindow(WindowName windowName);
        void RestoreWindow(WindowName windowName);
        void TransmittingValue<TValue>(TValue value, WindowName windowName, TransmittingParameter transmittingParameter);
    }
}