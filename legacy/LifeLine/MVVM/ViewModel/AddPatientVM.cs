using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using LifeLine.Services.NavigationPages;
using LifeLine.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace LifeLine.MVVM.ViewModel
{
    class AddPatientVM : BaseViewModel, IUpdatableWindow
    {
        private IServiceProvider _serviceProvider;

        private IDialogService _dialogService;
        private IDataBaseService _dataBaseService;
        private INavigationPage _navigationPage;

        public AddPatientVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dialogService = _serviceProvider.GetService<IDialogService>();
            _dataBaseService = _serviceProvider.GetService<IDataBaseService>();
            _navigationPage = _serviceProvider.GetRequiredService<INavigationPage>();

            GetPatients();
            GetGenders();
            GetDepartments();
            GetEmployees();
        }

        public void Update(object value)
        {

        }

        //------------------------------------------------------------------------------------------------

        #region Свойства

            #region Пагинация=

        private const int _NUMBER_ITEM_PAGE = 3;

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        private int _totalPage;
        public int TotalPage
        {
            get => _totalPage;
            set
            {
                _totalPage = value;
                OnPropertyChanged();
            }
        }

        #endregion

            #region Selected

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;

                if (value == null) { return; }

                SecondNameTB = value.SecondName;
                FirstNameTB = value.FirstName;
                LastNameTB = value.LastName;
                RoomNumberTB = (int)value.RoomNumber;

                SelectedGender = Genders.FirstOrDefault(x => x.IdGender == value.IdGenderNavigation.IdGender);
                SelectedDepartment = Departments.FirstOrDefault(x => x.IdDepartment == value.IdDepartmentNavigation.IdDepartment);
                SelectedEmployee = _employees.FirstOrDefault(x => x.IdEmployee == value.IdEmployee); // TODO : Не заполняется врач при выборе пациента

                OnPropertyChanged();
            }
        }

        private Gender _selectedGender;
        public Gender SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                OnPropertyChanged();
            }
        }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                if (value != null)
                {
                    LoadEmployees(value.IdDepartment);
                }
                OnPropertyChanged();
            }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        #endregion

            #region TextBox

        private string _searchPatientTB;
        public string SearchPatientTB
        {
            get => _searchPatientTB;
            set
            {
                _searchPatientTB = value;
                OnPropertyChanged();
            }
        }

        private string _secondNameTB;
        public string SecondNameTB
        {
            get => _secondNameTB;
            set
            {
                _secondNameTB = value;
                OnPropertyChanged();
            }
        }

        private string _firstNameTB;
        public string FirstNameTB
        {
            get => _firstNameTB;
            set
            {
                _firstNameTB = value;
                OnPropertyChanged();
            }
        }

        private string _lastNameTB;
        public string LastNameTB
        {
            get => _lastNameTB;
            set
            {
                _lastNameTB = value;
                OnPropertyChanged();
            }
        }

        private int _roomNumberTB;
        public int RoomNumberTB
        {
            get => _roomNumberTB;
            set
            {
                _roomNumberTB = value;
                OnPropertyChanged();
            }
        }

        #endregion

            #region List

        public List<Patient> _patients { get; set; } = [];
        public ObservableCollection<Patient> Patients { get; set; } = [];
        public ObservableCollection<Gender> Genders { get; set; } = [];
        public ObservableCollection<Department> Departments { get; set; } = [];
        private List<Employee> _employees { get; set; } = [];
        public ObservableCollection<Employee> Employees { get; set; } = [];

        #endregion

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Команды

        private RelayCommand _addPatientCommand;
        public RelayCommand AddPatientCommand { get => _addPatientCommand ??= new(obj => { AddPatientAsync(); }); }

        private RelayCommand _updatePatientCommand;
        public RelayCommand UpdatePatientCommand { get => _updatePatientCommand ??= new(obj => { UpdatePatientAsync(); }); }

        private RelayCommand _openProfilePatientCommand;
        public RelayCommand OpenProfilePatientCommand => _openProfilePatientCommand ??= new RelayCommand(OpenProfilePatient);

        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand { get => _previousPageCommand ??= new(obj => { CurrentPage--; LoadPatient(); }); }

        private RelayCommand _nextPageCommand;
        public RelayCommand NextPageCommand { get => _nextPageCommand ??= new(obj => { CurrentPage++; LoadPatient(); }); }

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Методы

        private async void AddPatientAsync()
        {
            if (string.IsNullOrWhiteSpace(SecondNameTB) && string.IsNullOrWhiteSpace(FirstNameTB) && string.IsNullOrWhiteSpace(LastNameTB))
            {
                _dialogService.ShowMessage("Вы не заполнили ФИО пациента!!");
                return;
            }
            if (string.IsNullOrWhiteSpace(RoomNumberTB.ToString()))
            {
                _dialogService.ShowMessage("Вы не указали палату пациента!!");
                return;
            }
            if (SelectedGender == null)
            {
                _dialogService.ShowMessage("Вы не указали пол пациента!!");
                return;
            }
            if (SelectedDepartment == null)
            {
                _dialogService.ShowMessage("Вы не указали отдел пациента!!");
                return;
            }
            if (SelectedEmployee == null)
            {
                _dialogService.ShowMessage("Вы не указали врача пациента!!");
                return;
            }

            Patient patient = new Patient()
            {
                SecondName = SecondNameTB,
                FirstName = FirstNameTB,
                LastName = LastNameTB,
                RoomNumber = RoomNumberTB,

                IdGender = SelectedGender.IdGender,
                IdDepartment = SelectedDepartment.IdDepartment,
                IdEmployee = SelectedEmployee.IdEmployee,
            };

            await _dataBaseService.AddAsync(patient);

            ClearingDataEntry();
            GetPatients();
        }

        private async void UpdatePatientAsync()
        {
            if (SelectedPatient == null) { return; }

            var patient = await _dataBaseService.FindIdAsync<Patient>(SelectedPatient.IdPatient);

            if (patient != null)
            {
                bool exist = await _dataBaseService.ExistsAsync<Patient>(x => x.RoomNumber == patient.RoomNumber);

                if (exist)
                {
                    if (_dialogService.ShowMessageButton($"Вы точно хотите изменить данные «{patient.SecondName} {patient.FirstName} {patient.LastName}»", "Предупреждение!!", MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        patient.SecondName = SecondNameTB;
                        patient.FirstName = FirstNameTB;
                        patient.LastName = LastNameTB;
                        patient.RoomNumber = RoomNumberTB;

                        patient.IdGender = SelectedGender.IdGender;
                        patient.IdDepartment = SelectedDepartment.IdDepartment;
                        patient.IdEmployee = SelectedEmployee.IdEmployee;

                        await _dataBaseService.UpdateAsync(patient);

                        ClearingDataEntry();
                        GetPatients();
                    }
                }
                else
                {
                    patient.SecondName = SecondNameTB;
                    patient.FirstName = FirstNameTB;
                    patient.LastName = LastNameTB;
                    patient.RoomNumber = RoomNumberTB;

                    patient.IdGender = SelectedGender.IdGender;
                    patient.IdDepartment = SelectedDepartment.IdDepartment;
                    patient.IdEmployee = SelectedEmployee.IdEmployee;

                    await _dataBaseService.UpdateAsync(patient);

                    ClearingDataEntry();
                    GetPatients();
                }
            }
        }

        private void OpenProfilePatient(object parameter)
        {
            if (parameter != null)
            {
                if (parameter is Patient patient)
                {
                    _navigationPage.NavigateTo("ProfileEmployee", patient);
                }
            }
        }

        private void GetPatients()
        {
            _patients.Clear();

            var querySearch = _dataBaseService.GetDataTable<Patient>(x => x
                    .Include(x => x.IdGenderNavigation)
                    .Include(x => x.IdDepartmentNavigation)
                    .Include(x => x.IdEmployeeNavigation));

            if (!string.IsNullOrWhiteSpace(SearchPatientTB))
            {
                string searchPatientLower = SearchPatientTB.ToLower();

                querySearch = querySearch
                    .Where(x =>
                        x.SecondName.ToLower().Contains(searchPatientLower) ||
                        x.FirstName.ToLower().Contains(searchPatientLower) ||
                        x.LastName.ToLower().Contains(searchPatientLower) ||
                        x.RoomNumber.ToString().Contains(searchPatientLower) ||
                        x.IdGenderNavigation.GenderName.ToLower().Contains(searchPatientLower) ||
                        x.IdDepartmentNavigation.DepartmentName.ToLower().Contains(searchPatientLower) ||
                        x.IdEmployeeNavigation.SecondName.ToLower().Contains(searchPatientLower) ||
                        x.IdEmployeeNavigation.FirstName.ToLower().Contains(searchPatientLower) ||
                        x.IdEmployeeNavigation.LastName.ToLower().Contains(searchPatientLower));
            }

            List<Patient> patientList = querySearch.ToList();

            _patients.AddRange(patientList);

            CalculateTotalPage(patientList.Count);
            LoadPatient();
        }

        private void CalculateTotalPage(int count)
        {
            TotalPage = (int)Math.Ceiling((double)count / _NUMBER_ITEM_PAGE);
        }

        private void LoadPatient()
        {
            Patients.Clear();

            var patient = _patients.Skip((CurrentPage - 1) * _NUMBER_ITEM_PAGE).Take(_NUMBER_ITEM_PAGE);

            foreach (var item in patient)
            {
                Patients.Add(item);
            }
        }

        private async void GetGenders()
        {
            Genders.Clear();

            var genders = await _dataBaseService.GetDataTableAsync<Gender>();

            foreach (var item in genders)
            {
                Genders.Add(item);
            }
        }

        private async void GetDepartments()
        {
            Departments.Clear();

            var departments = await _dataBaseService.GetDataTableAsync<Department>();

            foreach (var item in departments)
            {
                Departments.Add(item);
            }
        }

        private async void GetEmployees()
        {
            _employees.Clear();

            //var employees = await _dataBaseService.GetDataTableAsync<Employee>(/*x => x.Where(x => x.IdPositionNavigation.IdPositionListNavigation.IdDepartment == SelectedDepartment.IdDepartment)*/);
            _employees.AddRange(await _dataBaseService.GetDataTableAsync<Employee>(x => x.Include(x => x.IdPositionNavigation.IdPositionListNavigation)));
        }

        private void LoadEmployees(int id_deportment)
        {
            Employees.Clear();

            foreach (var item in _employees.Where(x => x.IdPositionNavigation.IdPositionListNavigation.IdDepartment == id_deportment))
            {
                Employees.Add(item);
            }
        }

        private void ClearingDataEntry()
        {
            SecondNameTB = string.Empty;
            FirstNameTB = string.Empty;
            LastNameTB = string.Empty;
            RoomNumberTB = 0;

            SelectedGender = null;
            SelectedDepartment = null;
            SelectedEmployee = null;
        }

        #endregion
    }
}
