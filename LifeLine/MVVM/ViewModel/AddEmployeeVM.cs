using LifeLine.MVVM.Models.MSSQL_DB;
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
        public AddEmployeeVM(IDialogService dialogService)
        {
            _dialogService = dialogService;

            EmployeeList = [];
            PositionList = [];
            GenderList = [];
            
            GetEmployeeData();
            GetPositionData();
            GetGenderData();
        }

        private readonly IDialogService _dialogService;

        #region Свойства

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

        private Position _comboBoxSelectedPositionList;
        public Position ComboBoxSelectedPositionList
        {
            get => _comboBoxSelectedPositionList;
            set
            {
                _comboBoxSelectedPositionList = value;
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

        public ObservableCollection<Employee> EmployeeList { get; set; }

        public ObservableCollection<Position> PositionList { get; set; }

        public ObservableCollection<Gender> GenderList { get; set; }

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Команды

        private RelayCommand _addEmployeeCommand;
        public RelayCommand AddEmployeeCommand { get => _addEmployeeCommand ??= new(obj => { AddEmployee(); }); }

        private RelayCommand _updateEmployeeCommand;
        public RelayCommand UpdateEmployeeCommand { get => _updateEmployeeCommand ??= new(obj => { UpdateEmployee(); }); }

        private RelayCommand _deleteEmployeeCommand;
        public RelayCommand DeleteEmployeeCommand => _deleteEmployeeCommand ??= new RelayCommand(DeleteEmployee);

        #endregion

        //------------------------------------------------------------------------------------------------

        #region Методы

        private void AddEmployee()
        {
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

            using (EmployeeManagementContext context = new())
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

                context.Add(employee);
                context.SaveChanges();

                ClearingDataEntry();
                GetEmployeeData();
            }
        }

        private void UpdateEmployee()
        {
            using (EmployeeManagementContext context = new())
            {
                if (SelectedEmployee == null) { return; }

                var warning = context.Employees.Where(x => x.IdEmployee != SelectedEmployee.IdEmployee);

                foreach (var item in warning)
                {
                    if (item.Login == SelectedEmployee.Login)
                    {
                        _dialogService.ShowMessage("Такой «логин» уже есть!!");
                        return;
                    }
                }

                var employeeToUpdate = context.Employees.Find(SelectedEmployee.IdEmployee);

                if (employeeToUpdate != null)
                {
                    if (_dialogService.ShowMessageButton($"Вы точно хотите изменить данные «{SelectedEmployee.SecondName} {SelectedEmployee.FirstName} {SelectedEmployee.LastName}»!", "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        if (SelectedEmployee == null) { return; }

                        employeeToUpdate.SecondName = TextBoxSecondName;
                        employeeToUpdate.FirstName = TextBoxFirstName;
                        employeeToUpdate.LastName = TextBoxLastName;

                        employeeToUpdate.IdPosition = ComboBoxSelectedPositionList.IdPosition;

                        employeeToUpdate.Salary = TextBoxSalary;

                        employeeToUpdate.IdGender = ComboBoxSelectedGender.IdGender;

                        employeeToUpdate.Login = TextBoxLogin;
                        employeeToUpdate.Password = TextBoxPassword;

                        context.SaveChanges();

                        ClearingDataEntry();
                        GetEmployeeData();
                    }
                }
                else
                {
                    _dialogService.ShowMessage("TEST", "Предупреждение!!!");
                    //MessageBox.Show("srgdtgadRG");
                }
            }
        }

        private void DeleteEmployee(object parametr)
        {
            using (EmployeeManagementContext context = new())
            {
                if (parametr is Employee employee)
                {
                    var deleteEmployee = context.Employees.Find(employee.IdEmployee);

                    if (deleteEmployee != null)
                    {
                        if (_dialogService.ShowMessageButton($"Вы точно хотите удалить сотрудника " +
                            $"«{employee.SecondName} {employee.FirstName} {employee.LastName}»!", 
                            "Предупреждение!!!", 
                            MessageButtons.YesNo) == MessageButtons.Yes)
                        {
                            context.Remove(deleteEmployee);
                            context.SaveChanges();

                            // TODO: Ошибка с удалением сотрудника : РЕШЕНА

                            ClearingDataEntry();
                            GetEmployeeData();
                        }
                    }
                }
            }
        }

        private void GetEmployeeData()
        {
            EmployeeList.Clear();

            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var employeeList = context.Employees
                    .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdAccessLevelNavigation)
                    .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                    .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation.IdDepartmentNavigation)
                    .Include(x => x.IdGenderNavigation)
                    .ToList();
                
                foreach (var item in employeeList)
                {
                    EmployeeList.Add(item);
                }
            }
        }

        private void GetPositionData()
        {
            PositionList.Clear();

            using (EmployeeManagementContext context = new())
            {
                var positionList = context.Positions
                    .Include(x => x.IdPositionListNavigation)
                    .ToList();

                foreach (var item in positionList)
                {
                    PositionList.Add(item);
                }
            }
        }

        private void GetGenderData()
        {
            GenderList.Clear();

            using (EmployeeManagementContext context = new())
            {
                var genderList = context.Genders.ToList();

                foreach (var item in genderList)
                {
                    GenderList.Add(item);
                }
            }
        }
        // Первый вариант
        //private async Task SearchEmployeeAsync()
        //{
            
        //}

        // Второй вариант
        private async void SearchEmployeeAsync()
        {
            using (EmployeeManagementContext context = new())
            {
                //var search = await 
                //    context.Employees
                //        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                //            .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation).ThenInclude(x => x.IdDepartmentNavigation)
                //                .Include(x => x.IdGenderNavigation)
                //                    .Where(x => x.SecondName.ToLower().Contains(SearchEmployeeTB))

                //    .Union(context.Employees
                //        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                //            .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation).ThenInclude(x => x.IdDepartmentNavigation)
                //                .Include(x => x.IdGenderNavigation)
                //                    .Where(x => x.FirstName.ToLower().Contains(SearchEmployeeTB)))

                //    .Union(context.Employees
                //        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                //            .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation).ThenInclude(x => x.IdDepartmentNavigation)
                //                .Include(x => x.IdGenderNavigation)
                //                    .Where(x => x.LastName.ToLower().Contains(SearchEmployeeTB)))

                //    .Union(context.Employees
                //        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                //            .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation).ThenInclude(x => x.IdDepartmentNavigation)
                //                .Include(x => x.IdGenderNavigation)
                //                    .Where(x => x.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName.ToLower().Contains(SearchEmployeeTB)))

                //    .Union(context.Employees
                //        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                //            .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation).ThenInclude(x => x.IdDepartmentNavigation)
                //                .Include(x => x.IdGenderNavigation)
                //                    .Where(x => x.IdPositionNavigation.IdPositionListNavigation.PositionListName.ToLower().Contains(SearchEmployeeTB)))

                //    .Union(context.Employees
                //        .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation)
                //            .Include(x => x.IdPositionNavigation).ThenInclude(x => x.IdPositionListNavigation).ThenInclude(x => x.IdDepartmentNavigation)
                //                .Include(x => x.IdGenderNavigation)
                //                    .Where(x => x.Login.ToLower().Contains(SearchEmployeeTB)))
                //    .ToListAsync();

                var search = await context.Employees
                        .Include(x => x.IdPositionNavigation)
                            .ThenInclude(x => x.IdPositionListNavigation)
                                .ThenInclude(x => x.IdDepartmentNavigation)
                        .Include(x => x.IdGenderNavigation)
                        .Where(x =>
                            x.SecondName.ToLower().Contains(SearchEmployeeTB) ||
                            x.FirstName.ToLower().Contains(SearchEmployeeTB) ||
                            x.LastName.ToLower().Contains(SearchEmployeeTB) ||
                            x.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName.ToLower().Contains(SearchEmployeeTB) ||
                            x.IdPositionNavigation.IdPositionListNavigation.PositionListName.ToLower().Contains(SearchEmployeeTB) ||
                            x.Login.ToLower().Contains(SearchEmployeeTB) ||
                            x.IdGenderNavigation.GenderName.ToLower().Contains(SearchEmployeeTB))
                        .ToListAsync();

                // TODO: Проблема с поиском, пропадает <<Отдел>> и <<Пол>> : РЕШЕНА + упрощен запрос

                App.Current.Dispatcher.Invoke(() =>
                {
                    EmployeeList.Clear();

                    foreach (var item in search)
                    {
                        EmployeeList.Add(item);
                    }
                });
            }
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

        #endregion
    }
}
