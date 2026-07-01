using LifeLine.HrPanel.Desktop.ViewModels.Pages;
using LifeLine.HrPanel.Desktop.Views.Pages;
using Shared.WPF.Services.NavigationService.Pages;
using System.Windows.Controls;

namespace LifeLine.HrPanel.Desktop.Factories.Pages
{
    public sealed class ManagementHospitalPageFactory(Func<ManagementHospitalPageVM> videModelFactory) : IPageFactory
    {
        public Page Create() => new ManagementHospitalPage() { DataContext = videModelFactory() };
    }
}
