using LifeLine.MVVM.Models.AppModel;
using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using LifeLine.Services.NavigationPages;
using LifeLine.Services.NavigationWindow;
using LifeLine.Utils.Enum;
using LifeLine.Utils.Helper;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;

namespace LifeLine.MVVM.ViewModel
{
    class ProfileAddDocumentEmployeePageVM : BaseViewModel, IUpdatablePage
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDialogService _dialogService;
        private readonly IDataBaseService _dataBaseService;
        private readonly INavigationPage _navigationPage;
        private readonly INavigationWindow _navigationWindow;
        private readonly IOpenFileDialogService _openFileDialogService;

        public ProfileAddDocumentEmployeePageVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dialogService = _serviceProvider.GetService<IDialogService>();
            _dataBaseService = _serviceProvider.GetService<IDataBaseService>();
            _navigationPage = _serviceProvider.GetService<INavigationPage>();
            _navigationWindow = _serviceProvider.GetService<INavigationWindow>();
            _openFileDialogService = _serviceProvider.GetService<IOpenFileDialogService>();

            GetTypeDocument();
        }

        public void Update(object value)
        {
            if (value is Employee employee)
            {
                CurrentUser = employee;
                GetDocument();
            }

            if (value is bool)
            {
                GetDocument();
            }

            if (value is Patient patient)
            {
                UserPatient = patient;
            }
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------

        #region Свойства

        private Employee _currentUser;
        public Employee CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private Patient _userPatient;
        public Patient UserPatient
        {
            get => _userPatient;
            set
            {
                _userPatient = value;
                OnPropertyChanged();
            }
        }

        private TypeDocument _selectedTypeDocument;
        public TypeDocument SelectedTypeDocument
        {
            get => _selectedTypeDocument;
            set
            {
                _selectedTypeDocument = value;
                OnPropertyChanged();
            }
        }

        private string _numberDocumentTB;
        public string NumberDocumentTB
        {
            get => _numberDocumentTB;
            set
            {
                _numberDocumentTB = value;
                OnPropertyChanged();
            }
        }

        private string _placeOfIssueTB;
        public string PlaceOfIssueTB
        {
            get => _placeOfIssueTB;
            set
            {
                _placeOfIssueTB = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateOfIssueDP;
        public DateTime? DateOfIssueDP
        {
            get => _dateOfIssueDP;
            set
            {
                _dateOfIssueDP = value;
                OnPropertyChanged();
            }
        }

        private string _documentFile;
        public string DocumentFile
        {
            get => _documentFile;
            set
            {
                _documentFile = value;
                OnPropertyChanged();
            }
        }

        private ImageDocumentEmployee _selectImage;
        public ImageDocumentEmployee SelectImage
        {
            get => _selectImage;
            set
            {
                _selectImage = value;
                OnPropertyChanged();
            }
        }

        private string _searchDocumentEmployeeTB;
        public string SearchDocumentEmployeeTB
        {
            get => _searchDocumentEmployeeTB;
            set
            {
                _searchDocumentEmployeeTB = value;
                SearchDocumentEmployeeAsync();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Document> Documents { get; set; } = [];
        public ObservableCollection<TypeDocument> TypeDocuments { get; set; } = [];
        public ObservableCollection<ImageDocumentEmployee> ImageDocumentEmployees { get; set; } = [];
        public List<Document> SelectedFiles { get; set; } = [];

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------------

        #region Команды

        private RelayCommand _addDocumentEmployeeCommand;
        public RelayCommand AddDocumentEmployeeCommand { get => _addDocumentEmployeeCommand ??= new(obj => { AddDocumentEmployee(); }); }

        private RelayCommand _deleteDocumentEmployeeCommand;
        public RelayCommand DeleteDocumentEmployeeCommand => _deleteDocumentEmployeeCommand ??= new RelayCommand(DeleteDocumentEmployee);

        private RelayCommand _deleteALLDocumentEmployeeCommand;
        public RelayCommand DeleteALLDocumentEmployeeCommand { get => _deleteALLDocumentEmployeeCommand ??= new(obj => { DeleteALLDocumentEmployee(); }); }

        private RelayCommand _backToProfileEmployeeCommand;
        public RelayCommand BackToProfileEmployeeCommand { get => _backToProfileEmployeeCommand ??= new(obj => { BackToProfileEmployee(); }); }

        private RelayCommand _selectedImageCommand;
        public RelayCommand SelectedImageCommand { get => _selectedImageCommand ??= new(obj => { SelectedImage(); }); }

        private RelayCommand _clearImageCommand;
        public RelayCommand ClearImageCommand { get => _clearImageCommand ??= new(obj => { ImageDocumentEmployees.Clear(); }); }

        private RelayCommand _deleteOneImageCommand;
        public RelayCommand DeleteOneImageCommand => _deleteOneImageCommand ??= new RelayCommand(DeleteOneImage);

        private RelayCommand _openDocumentWithUpdateCommand;
        public RelayCommand OpenDocumentWithUpdateCommand => _openDocumentWithUpdateCommand ??= new RelayCommand(OpenDocumentWithUpdate);

        private RelayCommand _openPreviewDocumentWithUpdateCommand;
        public RelayCommand OpenPreviewDocumentWithUpdateCommand => _openPreviewDocumentWithUpdateCommand ??= new RelayCommand(OpenPreviewDocumentWithUpdate);

        private RelayCommand _openDocumentNewWindowCommand;
        public RelayCommand OpenDocumentNewWindowCommand => _openDocumentNewWindowCommand ??= new RelayCommand(OpenDocumentNewWindow);

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------------

        #region Методы

        private async void AddDocumentEmployee()
        {
            DateOnly date;
            try
            {
                date = new DateOnly(DateOfIssueDP.Value.Year, DateOfIssueDP.Value.Month, DateOfIssueDP.Value.Day);
            }
            catch (Exception)
            {
                date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            if (SelectedTypeDocument == null)
            {
                _dialogService.ShowMessage("Вы не выбрали «Тип документа»!!");
                return;
            }

            foreach (var item in ImageDocumentEmployees)
            {
                Document document = new Document()
                {
                    IdEmployee = CurrentUser.IdEmployee,
                    IdTypeDocument = SelectedTypeDocument.IdTypeDocument,
                    Number = NumberDocumentTB,
                    PlaceOfIssue = PlaceOfIssueTB,
                    DateOfIssue = date,
                    DocumentImage = item.Image,
                    DocumentFile = item.NameImage,
                };

                await _dataBaseService.AddAsync(document);
            }

            GetDocument();
            ClearInputData();
        }

        private async void DeleteDocumentEmployee(object parameter)
        {
            if (parameter != null)
            {
                if (parameter is Document document)
                {
                    if (_dialogService.ShowMessageButton($"Вы действительно хотите удалить документ «{document.IdTypeDocumentNavigation.TypeDocumentName}»\n" +
                           $"у «{document.IdEmployeeNavigation.SecondName} {document.IdEmployeeNavigation.FirstName} {document.IdEmployeeNavigation.LastName}»!",
                           "Предупреждение!!", MessageButtons.YesNo) == MessageButtons.Yes)
                    {
                        await _dataBaseService.DeleteAsync(document);
                        GetDocument();
                    }
                }
            }
        }

        private async void DeleteALLDocumentEmployee()
        {
            if (SelectedFiles.Count > 1)
            {
                if (_dialogService.ShowMessageButton($"Вы действительно хотите удалить выбранные элементы «{SelectedFiles.Count}»!!", "Предупреждение!!", MessageButtons.YesNo) == MessageButtons.Yes)
                {
                    foreach (var item in SelectedFiles)
                    {
                        await _dataBaseService.DeleteAsync(item);
                    }
                    GetDocument();
                }
            }
            else
            {
                _dialogService.ShowMessage("Вы не выбрали ни одного элемента!!");
            }
        }

        private void BackToProfileEmployee()
        {
            _navigationPage.NavigateTo("ProfileEmployee", CurrentUser);
        }

        private void SelectedImage()
        {
            var files = _openFileDialogService.OpenDialog();
            if (files == null) { return; }

            if (files.Length != 0)
            {
                foreach (var item in files)
                {
                    byte[] imageByte;
                    FileInfo fileInfo = new FileInfo(item);

                    using (Image image = Image.FromFile(item))
                    {
                        imageByte = FileHelper.ImageToBytes(image);
                    }

                    ImageDocumentEmployee imageDocumentEmployee = new ImageDocumentEmployee();

                    imageDocumentEmployee.Image = imageByte;
                    imageDocumentEmployee.NameImage = fileInfo.Name;

                    ImageDocumentEmployees.Add(imageDocumentEmployee);
                }
            }
        }

        private void DeleteOneImage(object parameter)
        {
            if (parameter != null)
            {
                if (parameter is ImageDocumentEmployee imageDocumentEmployee)
                {
                    ImageDocumentEmployees.Remove(imageDocumentEmployee);
                }
            }
        }

        private void OpenDocumentWithUpdate(object parameter)
        {
            if (parameter != null)
            {
                if (parameter is Document imageDoc)
                {
                    List<object> obj = [imageDoc, CurrentUser.Avatar];

                    _navigationWindow.NavigateTo("PreviewDocumentWithUpdate", obj);
                }
            }
        }
        

        private void OpenPreviewDocumentWithUpdate(object parameter)
        {
            if (parameter != null)
            {
                if (parameter is ImageDocumentEmployee image)
                {
                    _navigationWindow.NavigateTo("PreviewDocumentWithUpdate", image);
                }
            }
        }

        private void OpenDocumentNewWindow(object parameter)
        {
            if (parameter != null)
            {
                if (parameter is Document imageDoc)
                {
                    List<object> obj = [imageDoc, CurrentUser.Avatar];
                    _navigationWindow.NavigateTo("PreviewDocumentNewWindow", obj);
                }
            }
        }

        private async void GetDocument()
        {
            Documents.Clear();

            var document = await _dataBaseService.GetDataTableAsync<Document>(x => x.Where(x => x.IdEmployee == CurrentUser.IdEmployee)
                                                                                    .Include(x => x.IdTypeDocumentNavigation)
                                                                                    .Include(x => x.IdEmployeeNavigation)
                                                                                    .OrderBy(x => x.IdTypeDocument));

            foreach (var item in document)
            {
                Documents.Add(item);
            }
        }

        private void ClearInputData()
        {
            SelectedTypeDocument = null;
            NumberDocumentTB = string.Empty;
            PlaceOfIssueTB = string.Empty;
            DateOfIssueDP = DateTime.Now;
            SelectImage = null;
            ImageDocumentEmployees.Clear();
        }

        private async void GetTypeDocument()
        {
            TypeDocuments.Clear();

            var typeDocument = await _dataBaseService.GetDataTableAsync<TypeDocument>();

            foreach (var item in typeDocument)
            {
                TypeDocuments.Add(item);
            }
        }

        private async void SearchDocumentEmployeeAsync()
        {
            //var search = 
            //    await 
            //        _dataBaseService.GetDataTableAsync<Document>(x => x
            //            .Include(x => x.IdTypeDocumentNavigation)
            //                .Where(x => x.IdTypeDocumentNavigation.TypeDocumentName.Contains(SearchDocumentEmployeeTB) ||
            //                            x.Number.Contains(SearchDocumentEmployeeTB) ||
            //                            x.PlaceOfIssue.ToLower().Contains(SearchDocumentEmployeeTB.ToLower()) ||
            //                            x.DateOfIssue.ToString().Contains(SearchDocumentEmployeeTB.ToString()) ||
            //                            x.DocumentFile.ToLower().Contains(SearchDocumentEmployeeTB)));
            
            var search = 
                await 
                    _dataBaseService.GetDataTableAsync<Document>(x => x
                        .Include(x => x.IdTypeDocumentNavigation)
                            .Where(x => x.IdTypeDocumentNavigation.TypeDocumentName.Contains(SearchDocumentEmployeeTB) ||
                                        x.DocumentFile.ToLower().Contains(SearchDocumentEmployeeTB)));

            App.Current.Dispatcher.Invoke(() =>
            {
                Documents.Clear();

                foreach (var item in search)
                {
                    Documents.Add(item);
                }
            });
        }

        #endregion
    }
}
