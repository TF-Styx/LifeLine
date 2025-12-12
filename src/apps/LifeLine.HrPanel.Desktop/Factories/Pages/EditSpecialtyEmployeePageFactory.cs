using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    public sealed class EditSpecialtyEmployeePageFactory(Func<EditSpecialtyEmployeePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EditSpecialtyEmployeePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EditSpecialtyEmployeePage() { DataContext = _viewModelFactory() };
    }
}
