using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    public sealed class EditWorkPermitEmployeePageFactory(Func<EditWorkPermitEmployeePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EditWorkPermitEmployeePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EditWorkPermitEmployeePage() { DataContext = _viewModelFactory() };
    }
}
