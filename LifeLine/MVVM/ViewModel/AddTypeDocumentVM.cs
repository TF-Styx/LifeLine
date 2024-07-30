using LifeLine.MVVM.Models.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeLine.MVVM.ViewModel
{
    class AddTypeDocumentVM : BaseViewModel
    {
        public AddTypeDocumentVM()
        {
            TypeDocumentList = [];
            TypeOfPersoneList = [];
            GetTypeOfPersonOnComboBox();
            GetTypeDocument();

            ComboBoxTypeOfPersone = TypeOfPersoneList.First();
        }


        #region Свойства

        private string _textBoxTypeDocuments;
        public string TextBoxTypeDocuments
        {
            get => _textBoxTypeDocuments;
            set
            {
                _textBoxTypeDocuments = value;
                OnPropertyChanged();
            }
        }

        private TypeOfPersone _comboBoxTypeOfPersone;
        public TypeOfPersone ComboBoxTypeOfPersone
        {
            get => _comboBoxTypeOfPersone;
            set
            {
                _comboBoxTypeOfPersone = value;
                OnPropertyChanged();
            }
        }

        private string _searchTypeDocumentLists;
        public string SearchTypeDocumentLists
        {
            get => _searchTypeDocumentLists;
            set
            {
                _searchTypeDocumentLists = value;
                SearchTypeDocumentListName();
                OnPropertyChanged();
            }
        }

        private TypeDocument _selectTypeDocumentLists;
        public TypeDocument SelectTypeDocumentLists
        {
            get => _selectTypeDocumentLists;
            set
            {
                _selectTypeDocumentLists = value;

                if (value == null)
                {
                    return;
                }

                TextBoxTypeDocuments = value.TypeDocumentName;
                ComboBoxTypeOfPersone = TypeOfPersoneList.FirstOrDefault(cbtop => cbtop.IdTypeOfPersone == SelectTypeDocumentLists.IdTypeOfPersone);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TypeDocument> TypeDocumentList { get; set; }

        public ObservableCollection<TypeOfPersone> TypeOfPersoneList { get; set; }

        #endregion


        #region Команды

        private RelayCommand _addTypeDocumentsCommand;
        public RelayCommand AddTypeDocumentsCommand { get => _addTypeDocumentsCommand ??= new(obj => { AddTypeDocuments(); }); }

        private RelayCommand _updateTypeDocumentsCommand;
        public RelayCommand UpdateTypeDocumentsCommand { get => _updateTypeDocumentsCommand ??= new(obj => { UpdateDepartament(); }); }

        private RelayCommand _deleteDepartmentCommand;
        public RelayCommand DeleteDepartmentCommand => _deleteDepartmentCommand ??= new RelayCommand(DeleteDepartment);

        #endregion


        #region Методы

        private void AddTypeDocuments()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (string.IsNullOrWhiteSpace(TextBoxTypeDocuments))
                {
                    MessageBox.Show("Вы не заполнили поле!!");
                    return;
                }
                if (context.TypeDocuments.Any(tdl => tdl.TypeDocumentName.ToLower() == TextBoxTypeDocuments.ToLower()))
                {
                    MessageBox.Show("Такое поле уже есть!!");
                }
                else
                {
                    if (ComboBoxTypeOfPersone.IdTypeOfPersone == 0)
                    {
                        MessageBox.Show("Вы не выбрали на кого будут записываться документы!!");
                        return;
                    }

                    TypeDocument typeDocument = new TypeDocument()
                    {
                        TypeDocumentName = TextBoxTypeDocuments,
                        IdTypeOfPersone = ComboBoxTypeOfPersone.IdTypeOfPersone,
                    };

                    context.TypeDocuments.Add(typeDocument);
                    context.SaveChanges();

                    TypeDocumentList.Clear();
                    TextBoxTypeDocuments = string.Empty;
                    GetTypeDocument();
                }
            }
        }

        private void UpdateDepartament()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (SelectTypeDocumentLists == null || ComboBoxTypeOfPersone.IdTypeOfPersone == 0)
                {
                    return;
                }

                var updateTypeDocumentLists = context.TypeDocuments.Find(SelectTypeDocumentLists.IdTypeDocument);

                if (updateTypeDocumentLists != null)
                {
                    if (context.TypeDocuments.Any(tdl => tdl.TypeDocumentName.ToLower() == TextBoxTypeDocuments.ToLower()) || string.IsNullOrWhiteSpace(TextBoxTypeDocuments))
                    {
                        MessageBox.Show($"Такой {SelectTypeDocumentLists.TypeDocumentName} уже есть!!\nИли пустой!!");
                    }
                    else
                    {
                        updateTypeDocumentLists.TypeDocumentName = TextBoxTypeDocuments;
                        updateTypeDocumentLists.IdTypeOfPersone = ComboBoxTypeOfPersone.IdTypeOfPersone;
                        context.SaveChanges();

                        TypeDocumentList.Clear();
                        TextBoxTypeDocuments = string.Empty;
                        GetTypeDocument();
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void DeleteDepartment(object parametr)
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                if (parametr is TypeDocument typeDocumentLists)
                {
                    var deleteTypeDocumentLists = context.TypeDocuments.Find(typeDocumentLists.IdTypeDocument);

                    if (MessageBox.Show($"Вы точно хотите удалить {typeDocumentLists.TypeDocumentName}?", "Предупреждение!!!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        context.Remove(typeDocumentLists);
                        context.SaveChanges();

                        TypeDocumentList.Clear();
                        GetTypeDocument();
                    }
                }
            }
        }

        private void GetTypeDocument()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var typeDocuments = context.TypeDocuments.Include(x => x.IdTypeOfPersoneNavigation).OrderBy(x => x.IdTypeOfPersone).ToList();

                foreach (var item in typeDocuments)
                {
                    TypeDocumentList.Add(item);
                }
            }
        }

        private void GetTypeOfPersonOnComboBox()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var typeOfPersone = context.TypeOfPersones.ToList();

                TypeOfPersoneList.Insert(0, new TypeOfPersone { TypeOfPersoneName = "Выберите чиь документы будут добавлятся!!" });

                foreach (var item in typeOfPersone)
                {
                    TypeOfPersoneList.Add(item);
                }
            }
        }

        private void SearchTypeDocumentListName()
        {
            using (EmployeeManagementContext context = new EmployeeManagementContext())
            {
                var searchTypeDocumentListName = context.TypeDocuments.Include(x => x.IdTypeOfPersoneNavigation).Where(stdl => stdl.TypeDocumentName.ToLower().Contains(SearchTypeDocumentLists.ToLower())).OrderBy(x => x.IdTypeOfPersone).ToList();

                TypeDocumentList.Clear();

                foreach (var item in searchTypeDocumentListName)
                {
                    TypeDocumentList.Add(item);
                }
            }
        }

        #endregion
    }
}
