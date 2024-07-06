using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.View.Windows;
using MasterAnalyticsDeadByDaylight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LifeLine.MVVM.ViewModel
{
    public class AuthWindowVM : BaseViewModel
    {
        private string _userLogin;
        public string UserLogin
        {
            get => _userLogin;
            set
            {
                _userLogin = value;
                OnPropertyChanged();
            }
        }

        private string _userPass;
        public string UserPass
        {
            get => _userPass;
            set
            {
                _userPass = value;
                OnPropertyChanged();
            }
        }

        public AuthWindowVM()
        {

        }

        private RelayCommand _loginButtonCommand;
        public RelayCommand LoginButtonCommand { get => _loginButtonCommand ??= new(obj => { Authorization(); });}

        private void Authorization()
        {
            using (EmployeeManagementContext context = new())
            {
                var id_user = context.Employees.FirstOrDefault(u => u.Login == UserLogin && u.Password == UserPass);

                if (id_user == null)
                {
                    MessageBox.Show("Не правльный логин или пароль!!");
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
            }
        }
    }
}
