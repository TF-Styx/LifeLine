using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace Shared.WPF.Services.NavigationService.Windows
{
    public sealed class NavigationWindow(IEnumerable<IWindowFactory> windowFactories) : INavigationWindow
    {
        private readonly Dictionary<WindowName, Window> _windows = [];
        private readonly Dictionary<string, IWindowFactory> _windowFactories = windowFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);

        public void OpenWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
            {
                window.Show();
            }
            else
            {
                Open(windowName);
            }
        }

        public void TransmittingValue<TValue>(TValue value, WindowName windowName, TransmittingParameter transmittingParameter)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
            {
                if (window.DataContext is IUpdatable updatable)
                {
                    updatable.Update(value, transmittingParameter);
                }
            }
        }

        private void Open(WindowName windowName)
        {
            if (_windowFactories.TryGetValue(windowName.ToString(), out IWindowFactory? windowFactory))
            {
                var window = windowFactory.Create();

                _windows.TryAdd(windowName, window);

                window.Closed += (sender, e) => _windows.Remove(windowName);

                window.Show();
            }
            else
            {
                throw new Exception("Данное фактори не зарегистрирована!");
            }
        }

        public void Close(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                window.Close();
        }

        public void MinimizeWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.MinimizeWindow(window);
        }

        public void MaximizeWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.MaximizeWindow(window);
        }

        public void RestoreWindow(WindowName windowName)
        {
            if (_windows.TryGetValue(windowName, out Window? window))
                SystemCommands.RestoreWindow(window);
        }
    }
}
