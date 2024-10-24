using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogService;
using LifeLine.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LifeLine.MVVM.ViewModel
{
    internal class AddEmployeeVM : BaseViewModel
    {
        public AddEmployeeVM(IDialogService dialogService, IDataBaseServices dataBaseServices)
        {
            _dialogService = dialogService;
            _dataBaseServices = dataBaseServices;
            IsCustomPopupCB = false;

            GetEmployeeData();
            GetPositionData();
            GetGenderData();
        }

        #region Popup

        private bool _isCustomPopupCB;
        public bool IsCustomPopupCB
        {
            get => _isCustomPopupCB;
            set
            {
                _isCustomPopupCB = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Свойства

        private readonly IDialogService _dialogService;

        private readonly IDataBaseServices _dataBaseServices;

        private string _textBoxSecondName;
        public string TextBoxSecondName
        {
            get => _textBoxSecondName;
            set
            {
                _textBoxSecondName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxFirstName;
        public string TextBoxFirstName
        {
            get => _textBoxFirstName;
            set
            {
                _textBoxFirstName = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxLastName;
        public string TextBoxLastName
        {
            get => _textBoxLastName;
            set
            {
                _textBoxLastName = value;
                OnPropertyChanged();
            }
        }

        private decimal? _textBoxSalary;
        public decimal? TextBoxSalary
        {
            get => _textBoxSalary;
            set
            {
                _textBoxSalary = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxLogin;
        public string TextBoxLogin
        {
            get => _textBoxLogin;
            set
            {
                _textBoxLogin = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxPassword;
        public string TextBoxPassword
        {
            get => _textBoxPassword;
            set
            {
                _textBoxPassword = value;
                OnPropertyChanged();
            }
        }

        private string _searchEmployeeTB;
        public string SearchEmployeeTB
        {
            get => _searchEmployeeTB;
            set
            {
                _searchEmployeeTB = value;
                // Первый вариант
                Task.Run(SearchEmployeeAsync);
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

                if (value == null)
                {
                    return;
                }

                TextBoxSecondName = value.SecondName;
                TextBoxFirstName = value.FirstName;
                TextBoxLastName = value.LastName;

                TextBoxSalary = value.Salary;

                TextBoxLogin = value.Login;
                TextBoxPassword = value.Password;

                ComboBoxSelectedPositionList = PositionList.FirstOrDefault(x => x.IdPositionList == value.IdPosition);
                ComboBoxSelectedGender = GenderList.FirstOrDefault(x => x.IdGender == value.IdGender);

                OnPropertyChanged();
            }
        }

        private string _depPos;
        public string DepPos
        {
            get => _depPos;
            set
            {
                _depPos = value;
                OnPropertyChanged();
            }
        }

        private Position _comboBoxSelectedPositionList;
        public Position ComboBoxSelectedPositionList
        {
            get => _comboBoxSelectedPositionList;
            set
            {
                _comboBoxSelectedPositionList = value;
                DepPos = $"Отдел: {value.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName}; Должность: {value.IdPositionListNavigation.PositionListName}";
                IsCustomPopupCB = false;
                OnPropertyChanged();
            }
        }

        private Gender _comboBoxSelectedGender;
        public Gender ComboBoxSelectedGender
        {
            get => _comboBoxSelectedGender;
            set
            {
                _comboBoxSelectedGender = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Employee> EmployeeList { get; set; } = [];

        public ObservableCollection<Position> PositionList { get; set; } = [];

        public ObservableCollection<Gender> GenderList { get; set; } = [];

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Команды

        private RelayCommand _addEmployeeCommand;
        public RelayCommand AddEmployeeCommand { get => _addEmployeeCommand ??= new(obj => { AddEmployeeAsync(); }); }

        private RelayCommand _updateEmployeeCommand;
        public RelayCommand UpdateEmployeeCommand { get => _updateEmployeeCommand ??= new(obj => { UpdateEmployeeAsync(); }); }

        private RelayCommand _deleteEmployeeCommand;
        public RelayCommand DeleteEmployeeCommand => _deleteEmployeeCommand ??= new RelayCommand(DeleteEmployeeAsync);

        private RelayCommand _openPopupPositionDepartment;
        public RelayCommand OpenPopupPositionDepartment { get => _openPopupPositionDepartment ??= new(obj => { OpenPopupPosDep(); }); }

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Методы

        private async void AddEmployeeAsync()
        {
            if (string.IsNullOrEmpty(TextBoxSecondName) && string.IsNullOrEmpty(TextBoxFirstName) && string.IsNullOrEmpty(TextBoxLastName))
            {
                _dialogService.ShowMessage("Вы не заполнили поля ФИО сотрудника!");
                return;
            }
            if (ComboBoxSelectedPositionList == null)
            {
                _dialogService.ShowMessage("Вы не выбрали должность сотрудника!");
                return;
            }
            if (TextBoxSalary == 0)
            {
                _dialogService.ShowMessage("Вы не назначили зароботную плату сотрудника!");
                return;
            }
            if (ComboBoxSelectedGender == null)
            {
                _dialogService.ShowMessage("Вы не указали пол сотрудника!");
                return;
            }
            if (string.IsNullOrEmpty(TextBoxLogin) && string.IsNullOrEmpty(TextBoxPassword))
            {
                _dialogService.ShowMessage("Вы не назначили логин и пароль сотрудника!");
                return;
            }
            if (await _dataBaseServices.ExistsAsync<Employee>(x => x.Login.ToLower() == TextBoxLogin.ToLower()))
            {
                _dialogService.ShowMessage("Такой логин уже занят!");
                return;
            }

            Employee employee = new()
            {
                SecondName = TextBoxSecondName,
                FirstName = TextBoxFirstName,
                LastName = TextBoxLastName,

                IdPosition = ComboBoxSelectedPositionList.IdPosition,

                Salary = TextBoxSalary,

                IdGender = ComboBoxSelectedGender.IdGender,

                Login = TextBoxLogin,
                Password = TextBoxPassword,
            };

            await _dataBaseServices.AddAsync(employee);

            ClearingDataEntry();
            GetEmployeeData();
        }

        private async void UpdateEmployeeAsync()
        {
            using (EmployeeManagementContext context = new())
            {
                if (SelectedEmployee == null) { return; }

                var employeeToUpdate = await _dataBaseServices.FindIdAsync<Employee>(SelectedEmployee.IdEmployee);

                if (employeeToUpdate != null)
                {
                    bool exists = await _dataBaseServices.ExistsAsync<Employee>(x => x.Login.ToLower() == employeeToUpdate.Login.ToLower());

                    if (exists)
                    {
                        if (_dialogService.ShowMessageButton($"Вы точно хотите изменить данные «{employeeToUpdate.SecondName} {employeeToUpdate.FirstName} {employeeToUpdate.LastName}»!", "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                        {
                            employeeToUpdate.SecondName = TextBoxSecondName;
                            employeeToUpdate.FirstName = TextBoxFirstName;
                            employeeToUpdate.LastName = TextBoxLastName;

                            employeeToUpdate.IdPosition = ComboBoxSelectedPositionList.IdPosition;

                            employeeToUpdate.Salary = TextBoxSalary;

                            employeeToUpdate.IdGender = ComboBoxSelectedGender.IdGender;

                            employeeToUpdate.Login = TextBoxLogin;
                            employeeToUpdate.Password = TextBoxPassword;

                            await _dataBaseServices.UpdateAsync(employeeToUpdate);

                            ClearingDataEntry();
                            GetEmployeeData();
                        }
                    }
                    else
                    {
                        employeeToUpdate.SecondName = TextBoxSecondName;
                        employeeToUpdate.FirstName = TextBoxFirstName;
                        employeeToUpdate.LastName = TextBoxLastName;

                        employeeToUpdate.IdPosition = ComboBoxSelectedPositionList.IdPosition;

                        employeeToUpdate.Salary = TextBoxSalary;

                        employeeToUpdate.IdGender = ComboBoxSelectedGender.IdGender;

                        employeeToUpdate.Login = TextBoxLogin;
                        employeeToUpdate.Password = TextBoxPassword;

                        await _dataBaseServices.UpdateAsync(employeeToUpdate);

                        ClearingDataEntry();
                        GetEmployeeData();
                    }
                }
            }
        }

        private async void DeleteEmployeeAsync(object parametr)
        {
            if (parametr != null)
            {
                if (parametr is Employee employee)
                {
                    if (_dialogService.ShowMessageButton($"Вы точно хотите удалить сотрудника " +
                            $"«{employee.SecondName} {employee.FirstName} {employee.LastName}»!",
                            "Предупреждение!!!",
                            MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        await _dataBaseServices.DeleteAsync(employee);

                        ClearingDataEntry();
                        GetEmployeeData();
                    }
                }
            }
        }

        private async void GetEmployeeData()
        {
            EmployeeList.Clear();

            var querySearch =
                    await _dataBaseServices.GetDataTableAsync<Employee>(x => x
                        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdAccessLevelNavigation)
                        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation.IdDepartmentNavigation)
                        .Include(x => x.IdGenderNavigation)

                        .Include(x => x.IdPositionNavigation)
                                .ThenInclude(x => x.IdPositionListNavigation)
                                    .ThenInclude(x => x.IdDepartmentNavigation)
                            .Include(x => x.IdGenderNavigation));

            if (!string.IsNullOrWhiteSpace(SearchEmployeeTB))
            {
                string searchLover = SearchEmployeeTB.ToLower();

                querySearch = 
                    querySearch.
                        Where(x =>
                            x.SecondName.ToLower().Contains(searchLover) ||
                            x.FirstName.ToLower().Contains(searchLover) ||
                            x.LastName.ToLower().Contains(searchLover) ||
                            x.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName.ToLower().Contains(searchLover) ||
                            x.IdPositionNavigation.IdPositionListNavigation.PositionListName.ToLower().Contains(searchLover) ||
                            x.Login.ToLower().Contains(searchLover) ||
                            x.IdGenderNavigation.GenderName.ToLower().Contains(searchLover));
            }

            List<Employee> employeeList = querySearch.ToList();

            foreach (var item in employeeList)
            {
                EmployeeList.Add(item);
            }
        }

        private async void GetPositionData()
        {
            PositionList.Clear();

            var positionList = await _dataBaseServices.GetDataTableAsync<Position>(x => x
                .Include(x => x.IdPositionListNavigation)
                .Include(x => x.IdPositionListNavigation.IdDepartmentNavigation));

            foreach (var item in positionList)
            {
                PositionList.Add(item);
            }
        }

        private async void GetGenderData()
        {
            GenderList.Clear();

            var genderList = await _dataBaseServices.GetDataTableAsync<Gender>();

            foreach (var item in genderList)
            {
                GenderList.Add(item);
            }
        }

        private async void SearchEmployeeAsync()
        {
            var search =
                await _dataBaseServices.GetDataTableAsync<Employee>(x => x
                        .Include(x => x.IdPositionNavigation)
                            .ThenInclude(x => x.IdPositionListNavigation)
                                .ThenInclude(x => x.IdDepartmentNavigation)
                        .Include(x => x.IdGenderNavigation)
                        .Where(x =>
                            x.SecondName.ToLower().Contains(SearchEmployeeTB.ToLower()) ||
                            x.FirstName.ToLower().Contains(SearchEmployeeTB.ToLower()) ||
                            x.LastName.ToLower().Contains(SearchEmployeeTB.ToLower()) ||
                            x.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName.ToLower().Contains(SearchEmployeeTB.ToLower()) ||
                            x.IdPositionNavigation.IdPositionListNavigation.PositionListName.ToLower().Contains(SearchEmployeeTB.ToLower()) ||
                            x.Login.ToLower().Contains(SearchEmployeeTB.ToLower()) ||
                            x.IdGenderNavigation.GenderName.ToLower().Contains(SearchEmployeeTB.ToLower())));

            App.Current.Dispatcher.Invoke(() =>
            {
                EmployeeList.Clear();

                foreach (var item in search)
                {
                    EmployeeList.Add(item);
                }
            });
        }

        private void ClearingDataEntry()
        {
            TextBoxSecondName = string.Empty;
            TextBoxFirstName = string.Empty;
            TextBoxLastName = string.Empty;

            ComboBoxSelectedPositionList = null;

            TextBoxSalary = 0;

            ComboBoxSelectedGender = null;

            TextBoxLogin = string.Empty;
            TextBoxPassword = string.Empty;
        }

        private void OpenPopupPosDep()
        {
            if (IsCustomPopupCB)
            {
                IsCustomPopupCB = false;
            }
            if (!IsCustomPopupCB)
            {
                IsCustomPopupCB = true;
            }
        }

        #endregion
    }
}
