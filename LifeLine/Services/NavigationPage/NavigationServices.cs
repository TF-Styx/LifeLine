using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LifeLine.Services.NavigationPage
{
    internal class NavigationServices : INavigationServices
    {
        private readonly Frame _frame;

        public NavigationServices(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(string namePage, object parametr = null)
        {
            switch (namePage)
            {
                case "ProfileEmployee":
                    _frame.Navigate(new ProfileEmployeePage(parametr));
                    break;

                default:
                    throw new ArgumentException("Страница не найдена");
            }
        }
    }
}
