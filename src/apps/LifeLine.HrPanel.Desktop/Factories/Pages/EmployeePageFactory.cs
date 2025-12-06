using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Shared.WPF.Enums;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    internal sealed class EmployeePageFactory
        (
            Func<EmployeePageVM> viewModelFactory,
            IServiceProvider serviceProvider
            //Func<INavigationPage> navigationPage
        ) : IPageFactory
    {
        private readonly Func<EmployeePageVM> _viewModelFactory = viewModelFactory;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        //private readonly Func<INavigationPage> _navigationPage = navigationPage;

        public Page Create()
        {
            var page = new EmployeePage() { DataContext = _viewModelFactory() };

            page.Loaded += (sender, args) =>
            {
                var navigationPage = _serviceProvider.GetRequiredService<INavigationPage>();

                if (navigationPage.CheckFrame(FrameName.EditContactInformationEmployeeFrame))
                    return;

                var frame = page.FindName(FrameName.EditContactInformationEmployeeFrame.ToString()) as Frame;
                //_navigationPage().SetFrame(FrameName.EditContactInformationEmployeeFrame, frame);
                navigationPage.SetFrame(FrameName.EditContactInformationEmployeeFrame, frame);
            };

            return page;
        }
    }
}
