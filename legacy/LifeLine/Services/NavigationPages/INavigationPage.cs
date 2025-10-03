using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LifeLine.Services.NavigationPages
{
    public interface INavigationPage
    {
        void NavigateTo(string namePage, object parameter);
        void GetFrame(Frame frame);
    }
}
