using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    internal sealed class EditDepartmentPositionPageFactory(Func<EditDepartmentPositionPageVM> viewModelFactory) : IPageFactory
    {
        public Page Create() => new EditDepartmentPositionPage() { DataContext = viewModelFactory() };
    }
}
