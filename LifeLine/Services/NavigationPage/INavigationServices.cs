using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.Services.NavigationPage
{
    interface INavigationServices
    {
        void NavigateTo(string namePage, object parametr);
    }
}
