using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogService;
using LifeLine.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddPositionListVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        private readonly IDataBaseServices _dataBaseServices;

        public AddPositionListVM(IDialogService dialogService, IDataBaseServices dataBaseServices)
        {
            PositionLists = [];
            DepartmentLists = [];

            _dialogService = dialogService;
            _dataBaseServices = dataBaseServices;

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
        private async void AddPositionLists()
        {
            if (string.IsNullOrEmpty(TextBoxPositionLists))
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
            else
            {
                PositionList positionList = new PositionList
                {
                    PositionListName = TextBoxPositionLists,
                    IdDepartment = SelectedDepartmentList.IdDepartment,
                };

                await _dataBaseServices.AddAsync(positionList);

                GetPositionList();
            }
        }

        private async void UpdatePositionLists()
        {
            if (SelectPositionList == null) { return; }

            var positionList = await _dataBaseServices.FindIdAsync<PositionList>(SelectPositionList.IdPositionList);

            if (positionList != null)
            {
                if (_dialogService.ShowMessageButton($"Вы точно хотите изменить {SelectPositionList.PositionListName}\nна\n{TextBoxPositionLists}",
                    "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                {
                    positionList.PositionListName = TextBoxPositionLists;
                    positionList.IdDepartment = SelectedDepartmentList.IdDepartment;

                    await _dataBaseServices.UpdateAsync(positionList);

                    PositionLists.Clear();
                    TextBoxPositionLists = string.Empty;

                    GetPositionList();
                }
            }
        }

        private void DeletePositionLists(object parametr)
        {
            if (parametr != null)
            {
                if (parametr is PositionList positionList)
                {
                    if (_dialogService.ShowMessageButton($"Вы точно хотите удалить {positionList.PositionListName}?", 
                        "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        _dataBaseServices.DeleteAsync(positionList);
                    }
                }
            }
        }

        /// <summary>
        /// Метод получения списка должностей
        /// </summary>
        private async void GetPositionList()
        {
            PositionLists.Clear();

            var querySearch = 
                await _dataBaseServices.GetDataTableAsync<PositionList>(x => x
                    .Include(x => x.IdDepartmentNavigation)
                        .OrderBy(x => x.IdDepartmentNavigation.DepartmentName));

            if (!string.IsNullOrWhiteSpace(SearchPositionList))
            {
                string searchLower = SearchPositionList.ToLower();

                querySearch = 
                    querySearch
                        .Where(x => x.PositionListName.ToLower().Contains(SearchPositionList.ToLower()) ||
                                    x.IdDepartmentNavigation.DepartmentName.ToLower().Contains(searchLower));
            }

            List<PositionList> positionList = querySearch.ToList();

            foreach (var item in positionList)
            {
                PositionLists.Add(item);
            }
        }

        /// <summary>
        /// Метод получения и заполнения данных для отделов
        /// </summary>
        private async void GetDepartmentList()
        {
            var departmentList = 
                await _dataBaseServices.GetDataTableAsync<Department>(x => x
                    .OrderBy(x => x.DepartmentName));

            foreach (var item in departmentList)
            {
                DepartmentLists.Add(item);
            }
        }

        /// <summary>
        /// Метод поиска по названию должности
        /// </summary>
        private async void SearchPositionListNameAsync()
        {
            var searchPositionListName = 
                await _dataBaseServices.GetDataTableAsync<PositionList>(x => x
                        .Include(x => x.IdDepartmentNavigation)
                            .Where(x => x.PositionListName.ToLower().Contains(SearchPositionList.ToLower()) ||
                                        x.IdDepartmentNavigation.DepartmentName.ToLower().Contains(SearchPositionList.ToLower())));

            App.Current.Dispatcher.Invoke(() =>
            {
                PositionLists.Clear();

                foreach (var item in searchPositionListName)
                {
                    PositionLists.Add(item);
                }
            });
        }

        #endregion

    }
}
