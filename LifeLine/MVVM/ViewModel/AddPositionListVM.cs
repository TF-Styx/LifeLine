using LifeLine.MVVM.Models.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Command;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddPositionListVM : BaseViewModel
    {
        public AddPositionListVM()
        {
            PositionLists = [];
            GetPositionList();
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
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PositionList> PositionLists { get; set; }

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
                if (context.PositionLists.Any(pl => pl.PositionListName.ToLower() == TextBoxPositionLists.ToLower()))
                {
                    MessageBox.Show("Такое поле уже есть!!");
                }
                else
                {
                    PositionList positionLists = new PositionList
                    {
                        PositionListName = TextBoxPositionLists,
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

                var updatePositionLists = context.PositionLists.Find(SelectPositionList.IdPositionList);

                if (updatePositionLists != null)
                {
                    if (context.PositionLists.Any(pl => pl.PositionListName.ToLower() == TextBoxPositionLists.ToLower()) || string.IsNullOrEmpty(TextBoxPositionLists))
                    {
                        MessageBox.Show($"Такой {SelectPositionList.PositionListName} уже есть!!\nИли пустой!!");
                    }
                    else
                    {
                        updatePositionLists.PositionListName = TextBoxPositionLists;
                        context.SaveChanges();

                        PositionLists.Clear();
                        TextBoxPositionLists = string.Empty;
                        GetPositionList();
                    }
                }
                else
                {
                    return;
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
                var positionList = context.PositionLists.ToList();

                foreach (var item in positionList)
                {
                    PositionLists.Add(item);
                }
            }
        }

        /// <summary>
        /// Метод поиска по названию должности
        /// </summary>
        private void SearchPositionListName()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var searchPositionListName = context.PositionLists.Where(spl => spl.PositionListName.ToLower().Contains(SearchPositionList.ToLower())).ToList();

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
