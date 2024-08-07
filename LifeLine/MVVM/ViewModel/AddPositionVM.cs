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

            GetPositionMainData();
            GetPositionData();
            GetDepartmentData();
            GetAccessData();
        }

        #region Свойства


        private Department _comboBoxSelectedDepartment;
        public Department ComboBoxSelectedDepartment
        {
            get => _comboBoxSelectedDepartment;
            set
            {
                _comboBoxSelectedDepartment = value;
                GetPositionData();
                OnPropertyChanged();
            }
        }

        private PositionList _comboBoxSelectedPosition;
        public PositionList ComboBoxSelectedPosition
        {
            get => _comboBoxSelectedPosition;
            set
            {
                _comboBoxSelectedPosition = value;
                OnPropertyChanged();
            }
        }

        private AccessLevel _comboBoxSelectedAccess;
        public AccessLevel ComboBoxSelectedAccess
        {
            get => _comboBoxSelectedAccess;
            set
            {
                _comboBoxSelectedAccess = value;
                OnPropertyChanged();
            }
        }

        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                if (value == null) { return; }

                _selectedPosition = value;

                ComboBoxSelectedDepartment = DepartmentList.FirstOrDefault(x => x.IdDepartment == value.IdPositionListNavigation.IdDepartment);
                ComboBoxSelectedPosition = PositionList.FirstOrDefault(x => x.IdPositionList == value.IdPositionList);
                ComboBoxSelectedAccess = AccessList.FirstOrDefault(x => x.IdAccessLevel == value.IdAccessLevel);

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

        private RelayCommand _updatePositionCommand;
        public RelayCommand UpdatePositionCommand { get => _updatePositionCommand ??= new(obj => { UpdatePosition(); }); }

        private RelayCommand _deletePositionCommand;
        public RelayCommand DeletePositionCommand => _deletePositionCommand ??= new RelayCommand(DeletePosition);

        #endregion


        #region Методы

        private void AddPosition()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (ComboBoxSelectedDepartment == null) { MessageBox.Show("Вы не выбрали отдел"); return; }
                if (ComboBoxSelectedPosition == null) { MessageBox.Show("Вы не выбрали должность"); return; }
                if (ComboBoxSelectedAccess == null) { MessageBox.Show("Вы не выбрали уровень доступа"); return; }

                Position position = new Position()
                {
                    IdPositionList = ComboBoxSelectedPosition.IdPositionList,
                    IdAccessLevel = ComboBoxSelectedAccess.IdAccessLevel
                };

                context.Positions.Add(position);
                context.SaveChanges();

                GetPositionMainData();
            }
        }

        private void UpdatePosition()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (SelectedPosition == null) { return; }

                var entityToUpdate = context.Positions.Find(SelectedPosition.IdPosition);

                if (entityToUpdate != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить: \n{SelectedPosition.IdPositionListNavigation.PositionListName}\nна\n{ComboBoxSelectedPosition.PositionListName}", "Предупреждение!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        entityToUpdate.IdPositionList = ComboBoxSelectedPosition.IdPositionList;
                        entityToUpdate.IdAccessLevel = ComboBoxSelectedAccess.IdAccessLevel;

                        context.SaveChanges();

                        GetPositionMainData();
                    }
                }
                else { return; }
            }
        }

        private void DeletePosition(object parametr)
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (parametr is Position position)
                {
                    //context.Positions.Find(position.IdPosition);

                    if (MessageBox.Show($"Вы точно хотиде удалить:\nДолжность: {position.IdPositionListNavigation.PositionListName}", "Предупреждение!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        context.Remove(position);
                        context.SaveChanges(); 
                        
                        GetPositionMainData();
                    }
                }
            }
        }

        private void GetPositionMainData()
        {
            PositionMainList.Clear();

            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var positionMainList = context.Positions.Include(x => x.IdPositionListNavigation).ThenInclude(x => x.IdDepartmentNavigation).Include(x => x.IdAccessLevelNavigation).ToList();

                foreach (var item in positionMainList)
                {
                    PositionMainList.Add(item);
                }
            }
        }

        private void GetPositionData()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                PositionList.Clear();

                if (ComboBoxSelectedDepartment == null)
                {
                    var positionList = context.PositionLists.ToList();

                    foreach (var item in positionList)
                    {
                        PositionList.Add(item);
                    }

                    PositionList.FirstOrDefault();
                }
                else
                {
                    var positionList = context.PositionLists.Where(x => x.IdDepartment == ComboBoxSelectedDepartment.IdDepartment).ToList();

                    foreach (var item in positionList)
                    {
                        PositionList.Add(item);
                    }

                    PositionList.FirstOrDefault();
                }
            }
        }

        private void GetDepartmentData()
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

        private void GetAccessData()
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
