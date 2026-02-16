using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Abstract;
using System.Windows.Controls;

namespace Shared.WPF.Services.NavigationService.Pages
{
    public sealed class NavigationPage(IEnumerable<IPageFactory> pageFactories) : INavigationPage
    {
        private readonly Dictionary<FrameName, PageName> _currentOpenPages = [];

        private readonly Dictionary<FrameName, Frame> _frames = [];
        private readonly Dictionary<PageName, Page> _pages = [];
        private readonly Dictionary<string, IPageFactory> _pageFactories = pageFactories.ToDictionary(f => f.GetType().Name.Replace("Factory", ""), f => f);

        public void SetFrame(FrameName frameName, Frame frame)
        {
            if (!_frames.TryAdd(frameName, frame))
                throw new Exception("Попытка добавления Frame в словарь - провалилась! Данный Frame уже существует!");
        }

        public bool CheckFrame(FrameName frameName) => _frames.Any(f => f.Key == frameName);

        public void NavigateTo(FrameName frameName, PageName pageName)
        {
            if (_pages.TryGetValue(pageName, out Page? page))
            {
                if (_frames.TryGetValue(frameName, out Frame? frame))
                {
                    frame.Navigate(page);
                    SetCurrentOpenPage(frameName, pageName);
                }
                else throw new Exception($"Данный {frameName} не найден! Вы не передали Frame! Возможно в Factory страницы, вы не передали Frame");
            }
            else
            {
                if (_pageFactories.TryGetValue(pageName.ToString(), out IPageFactory? pageFactory))
                {
                    if (_frames.TryGetValue(frameName, out Frame? frame))
                    {
                        var newPage = pageFactory.Create();
                        _pages.TryAdd(pageName, newPage);

                        frame.Navigate(newPage);
                        SetCurrentOpenPage(frameName, pageName);
                    }
                    else throw new Exception($"Данный {frameName} не найден! Вы не передали Frame! Возможно в Factory страницы, вы не передали Frame");
                }
                else throw new Exception($"Данный Factory для страницы {pageName} не найден!");
            }
        }

        public void TransmittingValue<TValue>(TValue value, FrameName frameName, PageName pageName, TransmittingParameter parameter)
        {
            if (_pages.TryGetValue(pageName, out Page? page))
            {
                if (_frames.TryGetValue(frameName, out Frame? frame))
                {
                    if (page.DataContext is IUpdatable viewModel)
                    {
                        viewModel.Update(value, parameter);
                        frame.Navigate(page);
                    }
                }
                else throw new Exception($"Данный {frameName} не найден! Вы не передали Frame! Возможно в Factory страницы, вы не передали Frame");
            }
            else
            {
                if (_pageFactories.TryGetValue(pageName.ToString(), out IPageFactory? pageFactory))
                {
                    if (_frames.TryGetValue(frameName, out Frame? frame))
                    {
                        var newPage = pageFactory.Create();
                        _pages.TryAdd(pageName, newPage);

                        frame.Navigate(newPage);
                        SetCurrentOpenPage(frameName, pageName);
                    }
                    else throw new Exception($"Данный {frameName} не найден! Вы не передали Frame! Возможно в Factory страницы, вы не передали Frame");
                }
                else throw new Exception($"Данный Factory для страницы {pageName} не найден!");
            }
        }

        public PageName GetCurrentOpenPage(FrameName frameName) => _currentOpenPages.GetValueOrDefault(frameName);

        private void SetCurrentOpenPage(FrameName frameName, PageName pageName) => _currentOpenPages.TryAdd(frameName, pageName);

        public void CloseAll()
            => _pages.Clear();
    }
}
