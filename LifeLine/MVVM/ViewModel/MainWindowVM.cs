using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.View.Windows;
using LifeLine.Services.DialogService;
using LifeLine.Services.NavigationPage;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
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

        public MainWindowVM(NavigationServices navigationServices, IDialogService dialogService)
        {
            _dialogService = dialogService;

            TextBlockMainWindowContentVisibility = Visibility.Collapsed;
            MainMenu = Visibility.Collapsed;
            MainGridVisibility = Visibility.Collapsed;

            //UserLogin = "pika";
            //UserPass = "pika";

            navigateS = navigationServices;
        }


        #region Свойства

        private readonly IDialogService _dialogService;


            #region Visibility

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

        private Visibility _mainGridVisibility;
        public Visibility MainGridVisibility
        {
            get => _mainGridVisibility;
            set
            {
                _mainGridVisibility = value;
                OnPropertyChanged();
            }
        }


                #region MenuVisibility

        private Visibility _mainMenu;
        public Visibility MainMenu
        {
            get => _mainMenu;
            set
            {
                _mainMenu = value;
                OnPropertyChanged();
            }
        }



        private Visibility _addEmployeeVisibility;
        public Visibility AddEmployeeVisibility
        {
            get => _addEmployeeVisibility;
            set
            {
                _addEmployeeVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addPatientVisibility;
        public Visibility AddPatientVisibility
        {
            get => _addPatientVisibility;
            set
            {
                _addPatientVisibility = value;
                OnPropertyChanged();
            }
        }



        private Visibility _addTypeDocumentVisibility;
        public Visibility AddTypeDocumentVisibility
        {
            get => _addTypeDocumentVisibility;
            set
            {
                _addTypeDocumentVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addDocumentEmployee;
        public Visibility AddDocumentEmployee
        {
            get => _addDocumentEmployee;
            set
            {
                _addDocumentEmployee = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addDocumentPatientVisibility;
        public Visibility AddDocumentPatientVisibility
        {
            get => _addDocumentPatientVisibility;
            set
            {
                _addDocumentPatientVisibility = value;
                OnPropertyChanged();
            }
        }



        private Visibility _addDepartmentVisibility;
        public Visibility AddDepartmentVisibility
        {
            get => _addDepartmentVisibility;
            set
            {
                _addDepartmentVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addPositionVisibility;
        public Visibility AddPositionVisibility
        {
            get => _addPositionVisibility;
            set
            {
                _addPositionVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addPositionListVisibility;
        public Visibility AddPositionListVisibility
        {
            get => _addDepartmentVisibility;
            set
            {
                _addDepartmentVisibility = value;
                OnPropertyChanged();
            }
        }



        private Visibility _addShiftVisibility;
        public Visibility AddShiftVisibility
        {
            get => _addShiftVisibility;
            set
            {
                _addShiftVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _addGraphVisibility;
        public Visibility AddGraphVisibility
        {
            get => _addDepartmentVisibility;
            set
            {
                _addDepartmentVisibility = value;
                OnPropertyChanged();
            }
        }



                    #region Словарь и методы сокрытия

        private void SetVisibility(string nameAccessLevel)
        {
            Dictionary<string, Action> dictionaryCollapsedInterface = new Dictionary<string, Action>
            {
                { "Администратор", () => AdminCollapsed() },
                { "Глав. Врач", () => GlavVrachCollapsed() },
                { "Заместитель", () => ZamestitelCollapsed() },
                { "Заведующий", () => ZaveduyshiylCollapsed() },
                { "Врач", () => VrachlCollapsed() },
                { "Главный мед брат", () => GlavMedBratCollapsed() },
                { "Старшая медсестра", () => StarshaiMedSestraCollapsed() },
                { "Медсестра / Медбрат", () => Medsestra_medbratCollapsed() },
                { "Младший мед. персонал", () => MladshiymedpersonalCollapsed() }
            };

            foreach (var item in dictionaryCollapsedInterface)
            {
                if (dictionaryCollapsedInterface.TryGetValue(nameAccessLevel, out Action action))
                {
                    action();
                }
            }
        }

        private void AdminCollapsed()
        {

        }

        private void GlavVrachCollapsed()
        {

        }

        private void ZamestitelCollapsed()
        {
            AddEmployeeVisibility = Visibility.Collapsed;
            AddTypeDocumentVisibility = Visibility.Collapsed;
            AddDepartmentVisibility = Visibility.Collapsed;
            AddPositionListVisibility = Visibility.Collapsed;
            AddShiftVisibility = Visibility.Collapsed;
        }

        private void ZaveduyshiylCollapsed()
        {
            AddEmployeeVisibility = Visibility.Collapsed;
            AddTypeDocumentVisibility = Visibility.Collapsed;
            AddDepartmentVisibility = Visibility.Collapsed;
            AddPositionListVisibility = Visibility.Collapsed;
            AddShiftVisibility = Visibility.Collapsed;
        }

        private void VrachlCollapsed()
        {
            AddEmployeeVisibility = Visibility.Collapsed;
            AddTypeDocumentVisibility = Visibility.Collapsed;
            AddDepartmentVisibility = Visibility.Collapsed;
            AddPositionVisibility = Visibility.Collapsed;
            AddPositionListVisibility = Visibility.Collapsed;
            AddShiftVisibility = Visibility.Collapsed;
        }

        private void GlavMedBratCollapsed()
        {
            AddEmployeeVisibility = Visibility.Collapsed;
            AddTypeDocumentVisibility = Visibility.Collapsed;
            AddDepartmentVisibility = Visibility.Collapsed;
            AddPositionVisibility = Visibility.Collapsed;
            AddPositionListVisibility = Visibility.Collapsed;
            AddShiftVisibility = Visibility.Collapsed;
        }

        private void StarshaiMedSestraCollapsed()
        {
            AddEmployeeVisibility = Visibility.Collapsed;
            AddTypeDocumentVisibility = Visibility.Collapsed;
            AddDepartmentVisibility = Visibility.Collapsed;
            AddPositionVisibility = Visibility.Collapsed;
            AddPositionListVisibility = Visibility.Collapsed;
            AddShiftVisibility = Visibility.Collapsed;
        }

        private void Medsestra_medbratCollapsed()
        {
            AddEmployeeVisibility = Visibility.Collapsed;
            AddTypeDocumentVisibility = Visibility.Collapsed;
            AddDepartmentVisibility = Visibility.Collapsed;
            AddPositionVisibility = Visibility.Collapsed;
            AddPositionListVisibility = Visibility.Collapsed;
            AddGraphVisibility = Visibility.Collapsed;
            AddShiftVisibility = Visibility.Collapsed;
        }

        private void MladshiymedpersonalCollapsed()
        {
            AddEmployeeVisibility = Visibility.Collapsed;
            AddTypeDocumentVisibility = Visibility.Collapsed;
            AddDepartmentVisibility = Visibility.Collapsed;
            AddPositionVisibility = Visibility.Collapsed;
            AddPositionListVisibility = Visibility.Collapsed;
            AddGraphVisibility = Visibility.Collapsed;
            AddShiftVisibility = Visibility.Collapsed;
        }

                    #endregion


                #endregion


            #endregion


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

        private RelayCommand _logOutOfAccountCommand;
        public RelayCommand LogOutOfAccountCommand { get => _logOutOfAccountCommand ??= new(obj => { LogOutOfAccount(); }); }

        #endregion


        #region Методы

        /// <summary>
        /// Метод входа
        /// </summary>
        private void Authorization()
        {
            using (EmployeeManagementContext context = new())
            {
                var id_user = 
                    context.Employees
                    .Include(x => x.IdPositionNavigation.IdAccessLevelNavigation)
                    .FirstOrDefault(u => u.Login == UserLogin && u.Password == UserPass);

                if (id_user == null)
                {
                    MessageBox.Show("Не правльный логин или пароль!!");
                }
                else
                {
                    CurrentUser = id_user;

                    StackPanelAuthVisibility = Visibility.Collapsed;
                    TextBlockAuthVisibility = Visibility.Collapsed;

                    TextBlockMainWindowContentVisibility = Visibility.Visible;
                    MainMenu = Visibility.Visible;
                    MainGridVisibility = Visibility.Visible;

                    SetVisibility(id_user.IdPositionNavigation.IdAccessLevelNavigation.AccessLevelName);
                }
            }
        }

        private void LogOutOfAccount()
        {
            StackPanelAuthVisibility = Visibility.Visible;
            TextBlockAuthVisibility = Visibility.Visible;

            TextBlockMainWindowContentVisibility = Visibility.Collapsed;
            MainMenu = Visibility.Collapsed;
            MainGridVisibility = Visibility.Collapsed;
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
