using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LifeLine.Services.NavigationWindow
{
    public interface INavigationWindow
    {
        void NavigateTo(string nameWindow, object parameter = null);
    }
}
