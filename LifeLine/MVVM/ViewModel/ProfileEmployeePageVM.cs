using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using LifeLine.Services.NavigationPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.MVVM.ViewModel
{
    class ProfileEmployeePageVM : BaseViewModel, IUpdatablePage
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDialogService _dialogService;
        private readonly IDataBaseService _dataBaseServices;

        public ProfileEmployeePageVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dialogService = serviceProvider.GetService<IDialogService>();
            _dataBaseServices = serviceProvider.GetService<IDataBaseService>();

            //GetUser(user);

            //GetEmployeeData();
            //GetTimeTable();
        }
        public void Update(object value)
        {
            if (value is Employee employee)
            {
                UserEmployee = employee;
                GetUser();
                GetEmployeeData();
                GetTimeTable();
            }
        }

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

        public ObservableCollection<Employee> Employees { get; set; } = [];
        public ObservableCollection<TimeTable> TimeTables { get; set; } = [];

        private void GetUser()
        {
            ImageProfile = UserEmployee.Avatar;
            DepartmentName = $"Отдел: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName}";
            DepartmentDescription = $"Описание отдела: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.Description}";
            DepartmentAddress = $"Адрес: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.Address}";
        }

        private void GetEmployeeData()
        {
            Employees.Clear();

            using (EmployeeManagementContext context = new())
            {
                var employeeData = context.Employees
                    .Where(x => x.IdPositionNavigation.IdPositionListNavigation.IdDepartment == UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartment && 
                           x.IdEmployee != UserEmployee.IdEmployee)
                    .ToList();

                foreach (var item in employeeData)
                {
                    Employees.Add(item);
                }
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
    }
}
