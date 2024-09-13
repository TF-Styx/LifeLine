using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DialogService;
using LifeLine.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Command;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddDepartmentWindowVM : BaseViewModel
    {
        public AddDepartmentWindowVM(IDialogService dialogService)
        {
            _dialogService = dialogService;

            DepartmentList = [];
            GetDataDepartment();
        }

        #region Свойства

        private readonly IDialogService _dialogService;

        private string _textBoxDepartment;
        public string TextBoxDepartment
        {
            get => _textBoxDepartment;
            set
            {
                _textBoxDepartment = value;
                OnPropertyChanged();
            }
        }

        private string _searchDepartment;
        public string SearchDepartment
        {
            get => _searchDepartment;
            set
            {
                _searchDepartment = value;
                SearchDepartmentName();
                OnPropertyChanged();
            }
        }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;

                if (value == null)
                {
                    return;
                }

                TextBoxDepartment = value.DepartmentName;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Department> DepartmentList {  get; set; }

        #endregion


        #region Команды

        private RelayCommand _addDepartmentCommand;
        public RelayCommand AddDepartmentCommand { get => _addDepartmentCommand ??= new(obj => { AddDepartment(); }); }

        private RelayCommand _updateDepartmentCommand;
        public RelayCommand UpdateDepartmentCommand { get => _updateDepartmentCommand ??= new(obj => { UpdateDepartament(); }); }

        private RelayCommand _deleteDepartmentCommand;
        public RelayCommand DeleteDepartmentCommand => _deleteDepartmentCommand ??= new RelayCommand(DeleteDepartment);

        #endregion


        #region Методы

        /// <summary>
        /// Медот добавления новой записи в таблицу отделов
        /// </summary>
        private void AddDepartment()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (string.IsNullOrWhiteSpace(TextBoxDepartment))
                {
                    _dialogService.ShowMessage("Вы не заполнили поле!!");
                    //MessageBox.Show("Вы не заполнили поле!!");
                    return;
                }
                if (context.Departments.Any(dl => dl.DepartmentName.ToLower() == TextBoxDepartment.ToLower())) 
                {
                    _dialogService.ShowMessage("Такое поле уже есть!!");
                    //MessageBox.Show("Такое поле уже есть!!");
                }
                else
                {
                    Department department = new Department
                    {
                        DepartmentName = TextBoxDepartment,
                    };

                    context.Departments.Add(department);
                    context.SaveChanges();

                    DepartmentList.Clear();
                    TextBoxDepartment = string.Empty;
                    GetDataDepartment();
                }
            }
        }

        /// <summary>
        /// Метод обновления данных в базе данных
        /// </summary>
        private void UpdateDepartament()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (SelectedDepartment == null)
                {
                    return;
                }

                var updateDepartament = context.Departments.Find(SelectedDepartment.IdDepartment);

                if (updateDepartament != null)
                {
                    if (context.Departments.Any(dl => dl.DepartmentName.ToLower() == TextBoxDepartment.ToLower()) || string.IsNullOrEmpty(TextBoxDepartment))
                    {
                        _dialogService.ShowMessage($"Такой {SelectedDepartment.DepartmentName} уже есть!!\nИли пустой!!");
                        //MessageBox.Show($"Такой {SelectedDepartment.DepartmentName} уже есть!!\nИли пустой!!");
                    }
                    else 
                    {
                        updateDepartament.DepartmentName = TextBoxDepartment;
                        context.SaveChanges();

                        DepartmentList.Clear();
                        TextBoxDepartment = string.Empty;
                        GetDataDepartment();
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Метод обновления данных
        /// </summary>
        /// <param name="parametr"></param>
        //private void UpdateDepartament(object parametr)
        //{
        //    using (EmployeeManagementContext context = new EmployeeManagementContext())
        //    {
        //        if (parametr is Department department)
        //        {
        //            var existingDepartment = context.Departments.Find(department.IdDepartment);

        //            if (existingDepartment != null)
        //            {
        //                // Проверяем, изменилось ли название отдела
        //                if (existingDepartment.DepartmentName != department.DepartmentName)
        //                {
        //                    // Обновляем название отдела
        //                    existingDepartment.DepartmentName = department.DepartmentName;

        //                    // Сохраняем изменения в базе данных
        //                    context.SaveChanges();

        //                    // Обновляем коллекцию отделов
        //                    DepartmentList.Clear();
        //                    GetDataDepartment();
        //                }
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Метод удаления отдела
        /// </summary>
        /// <param name="parametr">Выбраный элемент</param>
        private void DeleteDepartment(object parametr)
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (parametr is Department department)
                {
                    var deleteDepartment = context.Departments.Find(department.IdDepartment);

                    if (deleteDepartment != null)
                    {
                        
                        if (_dialogService.ShowMessageButton($"Вы точно хотите удалить {department.DepartmentName}?", "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                        {
                            context.Remove(deleteDepartment);
                            context.SaveChanges();

                            DepartmentList.Clear();
                            TextBoxDepartment = string.Empty;
                            GetDataDepartment();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Метод получения названия отделов, заполнения и вывод коллекции
        /// </summary>
        private void GetDataDepartment()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var department = context.Departments.ToList();

                foreach (var item in department)
                {
                    DepartmentList.Add(item);
                }
            }
        }

        /// <summary>
        /// Метод поиска по названию отдела
        /// </summary>
        private void SearchDepartmentName()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var searchDepartment = context.Departments.Where(d => d.DepartmentName.ToLower().Contains(SearchDepartment.ToLower())).ToList();
                
                DepartmentList.Clear();

                foreach (var item in searchDepartment)
                {
                    DepartmentList.Add(item);
                }
            }
        }

        #endregion
    }
}
