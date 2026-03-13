using LifeLine.AdminPanel.Desktop.ViewModels.Pages;
using LifeLine.AdminPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.AdminPanel.Desktop.Factories.Pages
{
    internal sealed class EditDepartmentPageFactory(Func<EditDepartmentPageVM> viewModelFactory) : IPageFactory
    {
        private readonly Func<EditDepartmentPageVM> _viewModelFactory = viewModelFactory;

        public Page Create() => new EditDepartmentPage() { DataContext = _viewModelFactory() };
    }
}
