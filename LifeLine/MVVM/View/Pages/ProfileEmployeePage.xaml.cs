using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LifeLine.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileEmployeePage.xaml
    /// </summary>
    public partial class ProfileEmployeePage : Page
    {
        public ProfileEmployeePage(object obj)
        {
            InitializeComponent();

            DataContext = new ProfileEmployeePageVM(obj);
        }
    }
}
