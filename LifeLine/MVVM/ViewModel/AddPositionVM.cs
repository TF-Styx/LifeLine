using LifeLine.MVVM.Models.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddPositionVM : BaseViewModel
    {
        public AddPositionVM()
        {
            PositionMainList = [];
            PositionList = [];
            DepartmentList = [];
            AccessList = [];

            GetPositionMainList();
            GetPositionList();
            GetDepartmentList();
            GetAccessList();
        }

        #region Свойства


        private Department _selectedDepartmentList;
        public Department SelectedDepartmentList
        {
            get => _selectedDepartmentList;
            set
            {
                _selectedDepartmentList = value;
                OnPropertyChanged();
            }
        }

        private PositionList _selectedPositionList;
        public PositionList SelectedPositionList
        {
            get => _selectedPositionList;
            set
            {
                _selectedPositionList = value;
                OnPropertyChanged();
            }
        }

        private AccessLevel _selectedAccessList;
        public AccessLevel SelectedAccessList
        {
            get => _selectedAccessList;
            set
            {
                _selectedAccessList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Position> PositionMainList { get; set; }

        public ObservableCollection<PositionList> PositionList { get; set; }

        public ObservableCollection<Department> DepartmentList { get; set; }

        public ObservableCollection<AccessLevel> AccessList { get; set; }

        #endregion


        #region Команды

        private RelayCommand _addPositionCommand;
        public RelayCommand AddPositionCommand { get => _addPositionCommand ??= new(obj => { AddPosition(); }); }

        #endregion


        #region Методы

        private void AddPosition()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (SelectedDepartmentList == null) { MessageBox.Show("Вы не выбрали отдел"); return; }
                if (SelectedPositionList == null) { MessageBox.Show("Вы не выбрали должность"); return; }
                if (SelectedAccessList == null) { MessageBox.Show("Вы не выбрали уровень доступа"); return; }

                Position position = new Position()
                {
                    IdPositionList = SelectedPositionList.IdPositionList,
                    IdAccessLevel = SelectedAccessList.IdAccessLevel
                };

                context.Positions.Add(position);
                context.SaveChanges();

                GetPositionMainList();
            }
        }

        private void GetPositionMainList()
        {
            PositionMainList.Clear();

            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var positionMainList = context.Positions.Include(x => x.IdPositionListNavigation).Include(x => x.IdAccessLevelNavigation).ToList();

                foreach (var item in positionMainList)
                {
                    PositionMainList.Add(item);
                }
            }
        }

        private void GetPositionList()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var positionList = context.PositionLists.ToList();

                foreach (var item in positionList)
                {
                    PositionList.Add(item);
                }
            }
        }

        private void GetDepartmentList()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var departmentList = context.Departments.ToList();

                foreach (var item in departmentList)
                {
                    DepartmentList.Add(item);
                }
            }
        }

        private void GetAccessList()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var accessList = context.AccessLevels.ToList();

                foreach(var item in accessList)
                {
                    AccessList.Add(item);
                }
            }
        }

        #endregion
    }
}
