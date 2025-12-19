using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    public sealed class EditAssignmentEmployeePageFactory(Func<EditAssignmentEmployeePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EditAssignmentEmployeePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EditAssignmentEmployeePage() { DataContext = _viewModelFactory() };
    }
}
