using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    public sealed class EditEducationDocumentEmployeePageFactory(Func<EditEducationDocumentEmployeePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EditEducationDocumentEmployeePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EditEducationDocumentEmployeePage() { DataContext = _viewModelFactory() };
    }
}
