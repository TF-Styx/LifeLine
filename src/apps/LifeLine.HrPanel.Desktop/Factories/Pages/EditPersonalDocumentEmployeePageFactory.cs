using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    public sealed class EditPersonalDocumentEmployeePageFactory(Func<EditPersonalDocumentEmployeePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EditPersonalDocumentEmployeePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EditPersonalDocumentEmployeePage() { DataContext = _viewModelFactory() };
    }
}
