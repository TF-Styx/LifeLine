﻿using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogService;
using LifeLine.Utils.Enum;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddDepartmentVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        private readonly IDataBaseServices _dataBaseServices;

        public AddDepartmentVM(IDialogService dialogService, IDataBaseServices dataBaseServices)
        {
            _dialogService = dialogService;
            _dataBaseServices = dataBaseServices;

            GetDepartmentDataAsync();
        }

        #region Свойства

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

        private string _textBoxAddress;
        public string TextBoxAddress
        {
            get => _textBoxAddress;
            set
            {
                _textBoxAddress = value;
                OnPropertyChanged();
            }
        }

        private string _textBoxDescription;
        public string TextBoxDescription
        {
            get => _textBoxDescription;
            set
            {
                _textBoxDescription = value;
                OnPropertyChanged();
            }
        }

        private string _searchDepartment;
        public string SearchDepartmentTB
        {
            get => _searchDepartment;
            set
            {
                _searchDepartment = value;
                SearchDepartmentNameAsync();
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
                TextBoxAddress = value.Address;
                TextBoxDescription = value.Description;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Department> DepartmentList { get; set; } = [];

        #endregion


        #region Команды

        private RelayCommand _addDepartmentCommand;
        public RelayCommand AddDepartmentCommand { get => _addDepartmentCommand ??= new(obj => { AddDepartmentAsync(); }); }

        private RelayCommand _updateDepartmentCommand;
        public RelayCommand UpdateDepartmentCommand { get => _updateDepartmentCommand ??= new(obj => { UpdateDepartamentAsync(); }); }

        private RelayCommand _deleteDepartmentCommand;
        public RelayCommand DeleteDepartmentCommand => _deleteDepartmentCommand ??= new RelayCommand(DeleteDepartment);

        #endregion


        #region Методы

        /// <summary>
        /// Медот добавления новой записи в таблицу отделов
        /// </summary>
        private async void AddDepartmentAsync()
        {
            if (string.IsNullOrWhiteSpace(TextBoxDepartment) || 
                string.IsNullOrWhiteSpace(TextBoxAddress) || 
                string.IsNullOrWhiteSpace(TextBoxDescription))
            {
                _dialogService.ShowMessage("Вы не заполнили поле!!");
                return;
            }
            if (await _dataBaseServices.ExistsAsync<Department>(x => x.DepartmentName.ToLower() == TextBoxDepartment.ToLower()))
            {
                _dialogService.ShowMessage("Такое поле уже есть!!");
            }
            else
            {
                Department department = new Department
                {
                    DepartmentName = TextBoxDepartment,
                    Address = TextBoxAddress,
                    Description = TextBoxDescription
                };

                await _dataBaseServices.AddAsync(department);

                GetDepartmentDataAsync();
            }
        }

        /// <summary>
        /// Метод обновления данных в базе данных
        /// </summary>
        private async void UpdateDepartamentAsync()
        {
            if (SelectedDepartment == null) { return; }

            var updateDepartament = await _dataBaseServices.FindIdAsync<Department>(SelectedDepartment.IdDepartment);

            if (updateDepartament != null)
            {
                bool exists = await _dataBaseServices.ExistsAsync<Department>(x => x.DepartmentName.ToLower() == TextBoxDepartment.ToLower());

                if (exists)
                {
                    if (string.IsNullOrWhiteSpace(TextBoxDepartment) ||
                        string.IsNullOrWhiteSpace(TextBoxAddress) ||
                        string.IsNullOrWhiteSpace(TextBoxDescription))
                    {
                        _dialogService.ShowMessage("Вы не заполнили поле!!");
                        return;
                    }
                    if (_dialogService.ShowMessageButton($"Вы точно хотите изменить {SelectedDepartment.DepartmentName}\nна\n{TextBoxDepartment}",
                    "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        updateDepartament.DepartmentName = TextBoxDepartment;
                        updateDepartament.Address = TextBoxAddress;
                        updateDepartament.Description = TextBoxDescription;
                        await _dataBaseServices.UpdateAsync(updateDepartament);

                        DepartmentList.Clear();
                        TextBoxDepartment = string.Empty;
                        TextBoxAddress = string.Empty;
                        TextBoxDescription = string.Empty;

                        GetDepartmentDataAsync();
                    }
                }
                else
                {
                    updateDepartament.DepartmentName = TextBoxDepartment;
                    updateDepartament.Address = TextBoxAddress;
                    updateDepartament.Description = TextBoxDescription;
                    await _dataBaseServices.UpdateAsync(updateDepartament);

                    DepartmentList.Clear();
                    TextBoxDepartment = string.Empty;
                    TextBoxAddress = string.Empty;
                    TextBoxDescription = string.Empty;

                    GetDepartmentDataAsync();
                }
            }
        }

        /// <summary>
        /// Метод удаления отдела
        /// </summary>
        /// <param name="parametr">Выбраный элемент</param>
        private void DeleteDepartment(object parametr)
        {
            if (parametr != null)
            {
                if (parametr is Department department)
                {
                    if (_dialogService.ShowMessageButton($"Вы точно хотите удалить {department.DepartmentName}?",
                        "Предупреждение!!!", MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        _dataBaseServices.DeleteAsync(department);

                        GetDepartmentDataAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Метод получения названия отделов, заполнения и вывод коллекции
        /// </summary>
        private async void GetDepartmentDataAsync()
        {
            DepartmentList.Clear();

            var query = 
                await _dataBaseServices.GetDataTableAsync<Department>(x => x.OrderBy(x => x.DepartmentName));

            if (!string.IsNullOrWhiteSpace(SearchDepartmentTB))
            {
                string searchLover = SearchDepartmentTB.ToLower();

                query =
                    query
                    .Where(x => x.DepartmentName.ToLower().Contains(searchLover) ||
                            x.Address.ToLower().Contains(searchLover));
            }

            List<Department> departmentList = query.ToList();

            foreach (var item in departmentList)
            {
                DepartmentList.Add(item);
            }
        }

        /// <summary>
        /// Метод поиска по названию отдела
        /// </summary>
        private async void SearchDepartmentNameAsync()
        {
            var searchDepartment = 
                await _dataBaseServices.GetDataTableAsync<Department>(x => x
                    .Where(x => x.DepartmentName.ToLower().Contains(SearchDepartmentTB.ToLower()) ||
                                x.Address.ToLower().Contains(SearchDepartmentTB.ToLower())));

            App.Current.Dispatcher.Invoke(() =>
            {
                DepartmentList.Clear();

                foreach (var item in searchDepartment)
                {
                    DepartmentList.Add(item);
                }
            });
        }

        #endregion
    }
}
