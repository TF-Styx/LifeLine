using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DialogService;
using LifeLine.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddPositionListVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        public AddPositionListVM(IDialogService dialogService)
        {
            PositionLists = [];
            DepartmentLists = [];

            GetPositionList();
            GetDepartmentList();

            _dialogService = dialogService;
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
                Task.Run(SearchPositionListNameAsync);
                //SearchPositionListName();
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
                    _dialogService.ShowMessage("Вы не заполнили поле!!", "Предупреждение!!!");
                    //MessageBox.Show("Вы не заполнили поле!!");
                    return;
                }
                if (SelectedDepartmentList == null)
                {
                    _dialogService.ShowMessage("Вы не выбрали отдел!!", "Предупреждение!!!");
                    //MessageBox.Show("Вы не выбрали отдел!!");
                    return;
                }
                if (context.PositionLists.Any(pl => pl.PositionListName.ToLower() == TextBoxPositionLists.ToLower()))
                {
                    _dialogService.ShowMessage("Такое поле уже есть!!", "Предупреждение!!!");
                    //MessageBox.Show("Такое поле уже есть!!");
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

                var warning = context.PositionLists.Where(x => x.IdPositionList != SelectPositionList.IdPositionList);

                foreach (var item in warning)
                {
                    if (item.PositionListName == SelectPositionList.PositionListName)
                    {
                        _dialogService.ShowMessage("Такая должность уже есть!!", "Предупреждение!!!");
                        //MessageBox.Show("Такая должность уже есть!!");
                        return;
                    }
                }

                var updatePositionLists = context.PositionLists.Find(SelectPositionList.IdPositionList);

                if (updatePositionLists != null)
                {
                    if (_dialogService.ShowMessageButton($"Вы точно хотите изменить {SelectPositionList.PositionListName}\nна\n{TextBoxPositionLists}", "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        if (SelectedDepartmentList == null)
                        {
                            //MessageBox.Show("Text");
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
                    _dialogService.ShowMessage("TEST", "Предупреждение!!!");
                    //MessageBox.Show("srgdtgadRG");
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
                        if (_dialogService.ShowMessageButton($"Вы точно хотите удалить {positionLists.PositionListName}?", "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                        {
                            //MessageBox.Show($"Вы точно хотите удалить {positionLists.PositionListName}?", "Предупреждение!!!", MessageBoxButtons.YesNo) == DialogResult.Yes
                            context.Remove(deletePositionLists);
                            context.SaveChanges();

                            // TODO: Ошибка с удалением списка должностей : РЕШЕНА

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
        private async void SearchPositionListNameAsync()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var searchPositionListName = await
                    context.PositionLists
                        .Include(x => x.IdDepartmentNavigation)
                            .Where(x => x.PositionListName.ToLower().Contains(SearchPositionList.ToLower()))
                    .Union(context.PositionLists
                        .Include(x => x.IdDepartmentNavigation)
                            .Where(x => x.IdDepartmentNavigation.DepartmentName.ToLower().Contains(SearchPositionList.ToLower())))
                    .ToListAsync();

                App.Current.Dispatcher.Invoke(() => 
                {
                    PositionLists.Clear();

                    foreach (var item in searchPositionListName)
                    {
                        PositionLists.Add(item);
                    }
                });
            }
        }

        #endregion

    }
}
