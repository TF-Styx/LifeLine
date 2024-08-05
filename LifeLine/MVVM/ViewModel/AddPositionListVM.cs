using LifeLine.MVVM.Models.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddPositionListVM : BaseViewModel
    {
        public AddPositionListVM()
        {
            PositionLists = [];
            DepartmentLists = [];
            GetPositionList();
            GetDepartmentList();
        }

        #region Свойства

        private string _textBoxPositionLists;
        public string TextBoxPositionLists
        {
            get => _textBoxPositionLists;
            set
            {
                _textBoxPositionLists = value;
                OnPropertyChanged();
            }
        }

        private string _searchPositionList;
        public string SearchPositionList
        {
            get => _searchPositionList;
            set
            {
                _searchPositionList = value;
                SearchPositionListName();
                OnPropertyChanged();
            }
        }

        private PositionList _selectPositionList;
        public PositionList SelectPositionList
        {
            get => _selectPositionList;
            set
            {
                _selectPositionList = value;

                if (value == null)
                {
                    return;
                }

                TextBoxPositionLists = value.PositionListName;
                SelectedDepartmentList = DepartmentLists.FirstOrDefault(x => x.IdDepartment == SelectPositionList.IdDepartment);
                OnPropertyChanged();
            }
        }

        private Department _selectedDepartmentList;
        public Department SelectedDepartmentList
        {
            get => _selectedDepartmentList;
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedDepartmentList = value;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<PositionList> PositionLists { get; set; }

        public ObservableCollection<Department> DepartmentLists { get; set; }

        #endregion


        #region Команды

        private RelayCommand _addPositionListsCommand;
        public RelayCommand AddPositionListsCommand { get => _addPositionListsCommand ??= new(obj => { AddPositionLists(); }); }

        private RelayCommand _updatePositionListsCommand;
        public RelayCommand UpdatePositionListsCommand { get => _updatePositionListsCommand ??= new(obj => { UpdatePositionLists(); }); }

        private RelayCommand _deletePositionListCommand;
        public RelayCommand DeletePositionListCommand => _deletePositionListCommand ??= new RelayCommand(DeletePositionLists);

        #endregion


        #region Методы

        /// <summary>
        /// Метод добавления списка должностей
        /// </summary>
        private void AddPositionLists()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (string.IsNullOrWhiteSpace(TextBoxPositionLists))
                {
                    MessageBox.Show("Вы не заполнили поле!!");
                    return;
                }
                if (SelectedDepartmentList == null)
                {
                    MessageBox.Show("Вы не выбрали отдел!!");
                    return;
                }
                if (context.PositionLists.Any(pl => pl.PositionListName.ToLower() == TextBoxPositionLists.ToLower()))
                {
                    MessageBox.Show("Такое поле уже есть!!");
                }
                else
                {
                    PositionList positionLists = new PositionList
                    {
                        PositionListName = TextBoxPositionLists,
                        IdDepartment = SelectedDepartmentList.IdDepartment,
                    };

                    context.PositionLists.Add(positionLists);
                    context.SaveChanges();

                    PositionLists.Clear();
                    TextBoxPositionLists = string.Empty;
                    GetPositionList();
                }
            }
        }

        private void UpdatePositionLists()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (SelectPositionList == null)
                {
                    return;
                }

                var a = context.PositionLists.Where(x => x.IdPositionList != SelectPositionList.IdPositionList);

                foreach (var item in a)
                {
                    if (item.PositionListName == SelectPositionList.PositionListName)
                    {
                        MessageBox.Show("Такая должность уже есть!!");
                        return;
                    }
                }

                var updatePositionLists = context.PositionLists.Find(SelectPositionList.IdPositionList);

                if (updatePositionLists != null)
                {
                    if (MessageBox.Show($"Вы точно хотите изменить {SelectPositionList.PositionListName}\nна\n{TextBoxPositionLists}", "Предупреждение!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (SelectedDepartmentList == null)
                        {
                            MessageBox.Show("Text");
                            return;
                        }

                        updatePositionLists.PositionListName = TextBoxPositionLists;
                        updatePositionLists.IdDepartment = SelectedDepartmentList.IdDepartment;
                        context.SaveChanges();

                        PositionLists.Clear();
                        TextBoxPositionLists = string.Empty;
                        GetPositionList();
                    }


                    //if (context.PositionLists.Any(pl => pl.PositionListName.ToLower() == SelectPositionList.PositionListName.ToLower()) || string.IsNullOrEmpty(TextBoxPositionLists))
                    //{
                    //    MessageBox.Show($"Такой {SelectPositionList.PositionListName} уже есть!!\nИли пустой!!");
                    //}
                    //else
                    //{
                    //    updatePositionLists.PositionListName = TextBoxPositionLists;
                    //    updatePositionLists.IdDepartment = SelectedDepartmentList.IdDepartment;
                    //    context.SaveChanges();

                    //    PositionLists.Clear();
                    //    TextBoxPositionLists = string.Empty;
                    //    GetPositionList();
                    //}
                }
                else
                {
                    MessageBox.Show("srgdtgadRG");
                }
            }
        }

        private void DeletePositionLists(object parametr)
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (parametr is PositionList positionLists)
                {
                    var deletePositionLists = context.PositionLists.Find(positionLists.IdPositionList);

                    if (deletePositionLists != null)
                    {
                        if (MessageBox.Show($"Вы точно хотите удалить {positionLists.PositionListName}?", "Предупреждение!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            context.Remove(positionLists);
                            context.SaveChanges();

                            PositionLists.Clear();
                            GetPositionList();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Метод получения списка должностей
        /// </summary>
        private void GetPositionList()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var positionList = context.PositionLists.Include(x => x.IdDepartmentNavigation).OrderBy(x => x.IdDepartmentNavigation.DepartmentName).ToList();

                foreach (var item in positionList)
                {
                    PositionLists.Add(item);
                }
            }
        }

        /// <summary>
        /// Метод получения и заполнения данных для отделов
        /// </summary>
        private void GetDepartmentList()
        {
            new Thread(() =>
            {
                using (EmployeeManagementContext context = new EmployeeManagementContext())
                {
                    var departmentList = context.Departments.ToList();

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in departmentList)
                        {
                            DepartmentLists.Add(item);
                        }
                    });
                }
            }).Start();
        }

        /// <summary>
        /// Метод поиска по названию должности
        /// </summary>
        private void SearchPositionListName()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var searchPositionListName = context.PositionLists.Include(x => x.IdDepartmentNavigation).Where(spl => spl.PositionListName.ToLower().Contains(SearchPositionList.ToLower())).ToList();

                PositionLists.Clear();

                foreach (var item in searchPositionListName)
                {
                    PositionLists.Add(item);
                }
            }
        }

        #endregion

    }
}
