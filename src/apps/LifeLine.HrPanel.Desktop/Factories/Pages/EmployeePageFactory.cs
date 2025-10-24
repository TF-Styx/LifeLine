using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    internal sealed class EmployeePageFactory(Func<EmployeePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EmployeePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EmployeePage() { DataContext = _viewModelFactory() };
    }
}
