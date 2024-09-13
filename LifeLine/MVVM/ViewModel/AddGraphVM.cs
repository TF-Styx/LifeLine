using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DialogService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.MVVM.ViewModel
{
    class AddGraphVM : BaseViewModel
    {
        public AddGraphVM(IDialogService dialogService)
        {
            _dialogService = dialogService;

            GetDepartmentData();
            //GetEmployeeData();

            SelectedDepartment = Departments.FirstOrDefault();
        }

        #region Свойства

        private readonly IDialogService _dialogService;


        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                GetSelectedEmployee();
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

        private string _searchDepartmentTB;
        public string SearchDepartmentTB
        {
            get => _searchDepartmentTB;
            set
            {
                _searchDepartmentTB = value;
                //SearchDepartmentAsync();
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
                SearchEmployeeAsync();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Department> Departments { get; set; } = [];

        public ObservableCollection<Employee> Employees { get; set; } = [];

        #endregion

        //---------------------------------------------------------------------------------------------------------

        #region Команды



        #endregion

        //---------------------------------------------------------------------------------------------------------

        #region Методы

        private void GetSelectedEmployee()
        {
            Employees.Clear();

            using (EmployeeManagementContext context = new())
            {
                var getSelectedEmployee = context.Employees.Where(x => x.IdPositionNavigation.IdPositionListNavigation.IdDepartment == SelectedDepartment.IdDepartment).ToList();

                foreach (var item in getSelectedEmployee)
                {
                    Employees.Add(item);
                }
            }
        }

        private void GetDepartmentData()
        {
            Departments.Clear();

            using (EmployeeManagementContext context = new())
            {
                var departaments = context.Departments.ToList();

                foreach (var item in departaments)
                {
                    Departments.Add(item);
                }
            }
        }

        //private void GetEmployeeData()
        //{
        //    Employees.Clear();

        //    using (EmployeeManagementContext context = new())
        //    {
        //        var employees = context.Employees.ToList();

        //        foreach (var item in employees)
        //        {
        //            Employees.Add(item);
        //        }
        //    }
        //}

        //private async void SearchDepartmentAsync()
        //{
        //    Departments.Clear();

        //    using (EmployeeManagementContext context = new())
        //    {
        //        var search = await context.Departments.Where(x => x.DepartmentName.ToLower().Contains(SearchDepartmentTB)).ToListAsync();

        //        foreach (var item in search)
        //        {
        //            Departments.Add(item);
        //        }
        //    }
        //}

        private async void SearchEmployeeAsync()
        {
            Employees.Clear();

            using (EmployeeManagementContext context = new())
            {
                var search = await context.Employees.Where(x => x.IdPositionNavigation.IdPositionListNavigation.IdDepartment == SelectedDepartment.IdDepartment)
                    .Where(x => x.SecondName.ToLower().Contains(SearchEmployeeTB) || 
                        x.FirstName.ToLower().Contains(SearchEmployeeTB) ||
                        x.LastName.ToLower().Contains(SearchEmployeeTB)).ToListAsync();

                foreach(var item in search)
                {
                    Employees.Add(item);
                }
            }
        }

        #endregion
    }
}
