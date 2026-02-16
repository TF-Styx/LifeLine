using Shared.WPF.Enums;
using System.Windows.Controls;

namespace Shared.WPF.Services.NavigationService.Pages
{
    public interface INavigationPage
    {
        PageName GetCurrentOpenPage(FrameName frameName);
        void NavigateTo(FrameName frameName, PageName pageName);
        void SetFrame(FrameName frameName, Frame frame);
        bool CheckFrame(FrameName frameName);
        void TransmittingValue<TValue>(TValue value, FrameName frameName, PageName pageName, TransmittingParameter parameter);
        void CloseAll();
    }
}