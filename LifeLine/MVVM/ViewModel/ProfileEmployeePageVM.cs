using LifeLine.MVVM.Models.MSSQL_DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.MVVM.ViewModel
{
    class ProfileEmployeePageVM : BaseViewModel
    {
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

        public ProfileEmployeePageVM(object user)
        {
            UserEmployee = (Employee)user;
            ImageProfile = UserEmployee.Avatar;
            DepartmentName = $"Отдел: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.DepartmentName}";
            DepartmentDescription = $"Описание отдела: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.Description}";
            DepartmentAddress = $"Адрес: {UserEmployee.IdPositionNavigation.IdPositionListNavigation.IdDepartmentNavigation.Address}";

            if (UserEmployee.Login == "qqndrik")
            {
                // TODO : Запрещать редактировать Никите фотографию
            }

            GetEmployeeData();
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
    }
}
