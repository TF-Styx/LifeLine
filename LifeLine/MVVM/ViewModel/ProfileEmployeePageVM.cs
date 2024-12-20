using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using LifeLine.Services.NavigationPages;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.MVVM.ViewModel
{
    class ProfileEmployeePageVM : BaseViewModel, IUpdatablePage
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDialogService _dialogService;
        private readonly IDataBaseService _dataBaseServices;
        private readonly INavigationPage _navigationPage;

        public ProfileEmployeePageVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dialogService = _serviceProvider.GetService<IDialogService>();
            _dataBaseServices = _serviceProvider.GetService<IDataBaseService>();
            _navigationPage = _serviceProvider.GetService<INavigationPage>();

            //GetUser(user);

            //GetEmployeeData();
            //GetTimeTable();
        }

        public void Update(object value)
        {
            if (value is int id_user)
            {
                if (UserEmployee.IdEmployee == id_user)
                {
                    GetTimeTable();
                }
            }

            if (value is Employee employee)
            {
                UserEmployee = employee;
                _currentUserRole = employee;

                EmployeeVisibilityManager();
                GetUser();
                GetEmployeeData();
                GetTimeTable();
            }

            if (value is Patient patient)
            {
                UserPatient = patient;
                _currentUserRole = patient;

                PatientVisibilityManager();
                GetPatient();
                GetPatientData();
            }
        }

        #region Visibility

        private Visibility _imageSPVisibility;
        public Visibility ImageSPVisibility
        {
            get => _imageSPVisibility;
            set
            {
                _imageSPVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _employeeFIOtbVisibility;
        public Visibility EmployeeFIOtbVisibility
        {
            get => _employeeFIOtbVisibility;
            set
            {
                _employeeFIOtbVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _patientFIOtbVisibility;
        public Visibility PatientFIOtbVisibility
        {
            get => _patientFIOtbVisibility;
            set
            {
                _patientFIOtbVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _timeTableSPVisibility;
        public Visibility TimeTableSPVisibility
        {
            get => _timeTableSPVisibility;
            set
            {
                _timeTableSPVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _colleaguesLWVisibility;
        public Visibility ColleaguesLWVisibility
        {
            get => _colleaguesLWVisibility;
            set
            {
                _colleaguesLWVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _patientsLWVisibility;
        public Visibility PatientsLWVisibility
        {
            get => _patientsLWVisibility;
            set
            {
                _patientsLWVisibility = value;
                OnPropertyChanged();
            }
        }

        private void PatientVisibilityManager()
        {
            ImageSPVisibility = Visibility.Collapsed;
            PatientFIOtbVisibility = Visibility.Visible;
            EmployeeFIOtbVisibility = Visibility.Collapsed;
            TimeTableSPVisibility = Visibility.Collapsed;
            ColleaguesLWVisibility = Visibility.Collapsed;
            PatientsLWVisibility = Visibility.Visible;
        }

        private void EmployeeVisibilityManager()
        {
            ImageSPVisibility = Visibility.Visible;
            PatientFIOtbVisibility = Visibility.Collapsed;
            EmployeeFIOtbVisibility = Visibility.Visible;
            TimeTableSPVisibility = Visibility.Visible;
            ColleaguesLWVisibility = Visibility.Visible;
            PatientsLWVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Свойства

        private object _currentUserRole = null;

        private Employee _userEmployee;
        public Employee UserEmployee
        {
            get => _userEmployee;
            set
            {
                _userEmployee = value;
                OnPropertyChanged();
            }
        }

        private Patient _userPatient;
        public Patient UserPatient
        {
            get => _userPatient;
            set
            {
                _userPatient = value;
                OnPropertyChanged();
            }
        }

            #region TextBlock

        private byte[] _imageProfile;
        public byte[] ImageProfile
        {
            get => _imageProfile;
            set
            {
                _imageProfile = value;
                OnPropertyChanged();
            }
        }

        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                _departmentName = value;
                OnPropertyChanged();
            }
        }

        private string _departmentDescription;
        public string DepartmentDescription
        {
            get => _departmentDescription;
            set
            {
                _departmentDescription = value;
                OnPropertyChanged();
            }
        }

        private string _departmentAddress;
        public string DepartmentAddress
        {
            get => _departmentAddress;
            set
            {
                _departmentAddress = value;
                OnPropertyChanged();
            }
        }

        #endregion

            #region List

        public ObservableCollection<Employee> Employees { get; set; } = [];
        public ObservableCollection<Patient> Patients { get; set; } = [];
        public ObservableCollection<TimeTable> TimeTables { get; set; } = [];

            #endregion

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Команды

        private RelayCommand _profileAddDocumentEmployeeCommand;
        public RelayCommand ProfileAddDocumentEmployeeCommand { get => _profileAddDocumentEmployeeCommand ??= new(obj => { OpenProfileAddDocumentEmployee(); }); }

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Методы

        private void OpenProfileAddDocumentEmployee()
        {
            Action action = _currentUserRole switch
            {
                Employee => () =>
                {
                    _navigationPage.NavigateTo("ProfileAddDocumentEmployee", _currentUserRole);
                },

                Patient => () =>
                {
                    _navigationPage.NavigateTo("ProfileAddDocumentEmployee", _currentUserRole);
                },

                _ => () => throw new Exception("Тип не определен!!")
            };
            action?.Invoke();

            //if (_currentUserRole is Employee employee)
            //{
            //    _navigationPage.NavigateTo("ProfileAddDocumentEmployee", UserEmployee);
            //}
            //if (_currentUserRole is Patient patient)
            //{
            //    _navigationPage.NavigateTo("ProfileAddDocumentEmployee", UserPatient);
            //}
        }

        private void GetUser()
        {
            ImageProfile = UserEmployee.Avatar;
            DepartmentName = $"Отдел: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName}";
            DepartmentDescription = $"Описание отдела: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.Description}";
            DepartmentAddress = $"Адрес: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.Address}";
        }

        private void GetPatient()
        {
            DepartmentName = $"Отдел: {UserPatient.IdDepartmentNavigation.DepartmentName}";
            DepartmentDescription = $"Описание отдела: {UserPatient.IdDepartmentNavigation.Description}";
            DepartmentAddress = $"Адрес: {UserPatient.IdDepartmentNavigation.Address}";
        }

        private async void GetEmployeeData()
        {
            Employees.Clear();

            var employeeData =
                await _dataBaseServices.GetDataTableAsync<Employee>(x => x
                    .Where(x => x.IdPositionNavigation.IdPositionListNavigation.IdDepartment == UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartment &&
                        x.IdEmployee != UserEmployee.IdEmployee));

            foreach (var item in employeeData)
            {
                Employees.Add(item);
            }
        }

        private async void GetPatientData()
        {
            Patients.Clear();

            var patientData =
                await _dataBaseServices.GetDataTableAsync<Patient>(x => x
                    .Include(x => x.IdDepartmentNavigation)
                        .Where(x => x.IdDepartment == UserPatient.IdDepartment && x.IdPatient != UserPatient.IdPatient));

            foreach (var item in patientData)
            {
                Patients.Add(item);
            }
        }

        private async void GetTimeTable()
        {
            TimeTables.Clear();

            //var timeTable = await _dataBaseServices.GetDataTableAsync<TimeTable>(x => x.Include(x => x.IdShiftNavigation).Where(x => x.IdEmployee == UserEmployee.IdEmployee).Where(x => x.Date <= DateTime.Today.AddDays(6)));

            var timeTable = await _dataBaseServices.GetDataTableAsync<TimeTable>(x => x.Include(x => x.IdShiftNavigation).Where(x => x.IdEmployee == UserEmployee.IdEmployee && x.Date >= DateTime.Now));

            foreach (var item in timeTable)
            {
                TimeTables.Add(item);
            }
        }

        #endregion
    }
}
