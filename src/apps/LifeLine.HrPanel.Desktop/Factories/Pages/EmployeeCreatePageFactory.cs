using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    internal sealed class EmployeeCreatePageFactory(Func<EmployeeCreatePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EmployeeCreatePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EmployeeCreatePage() { DataContext = _viewModelFactory() };
    }
}
