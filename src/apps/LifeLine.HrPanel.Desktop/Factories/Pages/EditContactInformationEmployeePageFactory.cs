using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    public sealed class EditContactInformationEmployeePageFactory(Func<EditContactInformationEmployeePageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EditContactInformationEmployeePageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EditContactInformationEmployeePage() { DataContext = _viewModelFactory() };
    }
}
