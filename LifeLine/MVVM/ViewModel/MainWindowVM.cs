using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.View.Windows;
using LifeLine.Services.NavigationPage;
using MasterAnalyticsDeadByDaylight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace LifeLine.MVVM.ViewModel
{
    internal class MainWindowVM : BaseViewModel
    {

        NavigationServices navigateS;
        Employee CurrentUser;

        public MainWindowVM(NavigationServices navigationServices)
        {
            StackPanelMainContentVisibility = Visibility.Collapsed;
            GridMainTopButtonContentVisibility = Visibility.Collapsed;
            TextBlockMainWindowContentVisibility = Visibility.Collapsed;

            UserLogin = "pika";
            UserPass = "pika";

            navigateS = navigationServices;
        }

        #region Свойства

        private Visibility _stackPanelAuthVisibility;
        public Visibility StackPanelAuthVisibility
        {
            get => _stackPanelAuthVisibility;
            set
            {
                _stackPanelAuthVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _stackPanelMainContentVisibility;
        public Visibility StackPanelMainContentVisibility
        {
            get => _stackPanelMainContentVisibility;
            set
            {
                _stackPanelMainContentVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _gridMainTopButtonContentVisibility;
        public Visibility GridMainTopButtonContentVisibility
        {
            get => _gridMainTopButtonContentVisibility;
            set
            {
                _gridMainTopButtonContentVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _textBlockAuthVisibility;
        public Visibility TextBlockAuthVisibility
        {
            get => _textBlockAuthVisibility;
            set
            {
                _textBlockAuthVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _textBlockMainWindowContentVisibility;
        public Visibility TextBlockMainWindowContentVisibility
        {
            get => _textBlockMainWindowContentVisibility;
            set
            {
                _textBlockMainWindowContentVisibility = value;
                OnPropertyChanged();
            }
        }



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

        #endregion


        #region Команды

        /// <summary>
        /// Команда проверки пользователя
        /// </summary>
        private RelayCommand _loginButtonCommand;
        public RelayCommand LoginButtonCommand { get => _loginButtonCommand ??= new(obj => { Authorization(); }); }

        /// <summary>
        /// Команда загрузи Frame
        /// </summary>
        private RelayCommand _openProfileEmployeePageCommand;
        public RelayCommand OpenProfileEmployeePageCommand { get => _openProfileEmployeePageCommand ??= new(obj => { OpenProfileEmployeePage(); }); }

        private RelayCommand _openAddDepartmentWindowCommand;
        public RelayCommand OpenAddDepartmentWindowCommand { get => _openAddDepartmentWindowCommand ??= new(obj => { OpenAddDepartmentWindow(); }); }

        private RelayCommand _openAddPositionListWindowCommand;
        public RelayCommand OpenAddPositionListWindowCommand { get => _openAddPositionListWindowCommand ??= new(obj => { OpenAddPositionListWindow(); }); }

        private RelayCommand _openAddTypeDocumentWindowCommand;
        public RelayCommand OpenAddTypeDocumentWindowCommand { get => _openAddTypeDocumentWindowCommand ??= new(obj => { OpenAddTypeDocumentWindow(); }); }

        private RelayCommand _openAddPositionWindowCommand;
        public RelayCommand OpenAddPositionWindowCommand { get => _openAddPositionWindowCommand ??= new(obj => { OpenAddPositionWindow(); }); }

        private RelayCommand _openAddEmployeeWindowCommand;
        public RelayCommand OpenAddEmployeeWindowCommand { get => _openAddEmployeeWindowCommand ??= new(obj => { OpenAddEmployeeWindow(); }); }
        
        private RelayCommand _openAddGraphWindowCommand;
        public RelayCommand OpenAddGraphWindowCommand { get => _openAddGraphWindowCommand ??= new(obj => { OpenAddGraphWindow(); }); }

        #endregion


        #region Методы

        /// <summary>
        /// Метод входа
        /// </summary>
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
                    CurrentUser = id_user;

                    StackPanelAuthVisibility = Visibility.Collapsed;
                    StackPanelMainContentVisibility = Visibility.Visible;
                    GridMainTopButtonContentVisibility = Visibility.Visible;
                    TextBlockAuthVisibility = Visibility.Collapsed;
                    TextBlockMainWindowContentVisibility = Visibility.Visible;
                }
            }
        }


        /// <summary>
        /// Метод загрузки странички в Frame
        /// </summary>
        private void OpenProfileEmployeePage()
        {
            navigateS.NavigateTo("ProfileEmployee", CurrentUser);
        }

        private void OpenAddDepartmentWindow()
        {
            AddDepartmentWindow addDepartmentWindow = new AddDepartmentWindow();
            addDepartmentWindow.Show();
        }

        private void OpenAddPositionListWindow()
        {
            AddPositionListWindow addPositionListWindow = new AddPositionListWindow();
            addPositionListWindow.Show();
        }

        private void OpenAddTypeDocumentWindow()
        {
            AddTypeDocumentWindow addTypeDocumentWindow = new AddTypeDocumentWindow();
            addTypeDocumentWindow.Show();
        }

        private void OpenAddPositionWindow()
        {
            AddPositionWindow addPositionWindow = new AddPositionWindow();
            addPositionWindow.Show();
        }

        private void OpenAddEmployeeWindow()
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.Show();
        }

        private void OpenAddGraphWindow()
        {
            AddGraphWindow addGraphWindow = new AddGraphWindow();
            addGraphWindow.Show();
        }

        #endregion
    }
}
