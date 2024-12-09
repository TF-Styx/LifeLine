using LifeLine.MVVM.Models.AppModel;
using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using LifeLine.Services.NavigationPages;
using LifeLine.Utils.Helper;
using MasterAnalyticsDeadByDaylight.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;

namespace LifeLine.MVVM.ViewModel
{
    class ProfileAddDocumentEmployeePageVM : BaseViewModel, IUpdatablePage
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDialogService _dialogService;
        private readonly IDataBaseService _dataBaseService;
        private readonly INavigationPage _navigationPage;
        private readonly IOpenFileDialogService _openFileDialogService;

        public ProfileAddDocumentEmployeePageVM(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _dialogService = _serviceProvider.GetService<IDialogService>();
            _dataBaseService = _serviceProvider.GetService<IDataBaseService>();
            _navigationPage = _serviceProvider.GetService<INavigationPage>();
            _openFileDialogService = _serviceProvider.GetRequiredService<IOpenFileDialogService>();

            GetTypeDocument();
        }

        public void Update(object value)
        {
            if (value is Employee employee)
            {
                CurrentUser = employee;
                GetDocument();
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

        private decimal? _numberDocumentTB;
        public decimal? NumberDocumentTB
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

        private byte[] _selectImage;
        public byte[] SelectImage
        {
            get => _selectImage;
            set
            {
                _selectImage = value;
                
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Document> Documents { get; set; } = [];
        public ObservableCollection<TypeDocument> TypeDocuments { get; set; } = [];
        public ObservableCollection<ImageDocumentEmployee> ImageDocumentEmployees { get; set; } = [];

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------------

        #region Команды

        private RelayCommand _addDocumentEmployeeCommand;
        public RelayCommand AddDocumentEmployeeCommand { get => _addDocumentEmployeeCommand ??= new(obj => { AddDocumentEmployee(); }); }

        private RelayCommand _updateDocumentEmployeeCommand;
        public RelayCommand UpdateDocumentEmployeeCommand { get => _updateDocumentEmployeeCommand ??= new(obj => { UpdateDocumentEmployee(); }); }

        private RelayCommand _backToProfileEmployeeCommand;
        public RelayCommand BackToProfileEmployeeCommand { get => _backToProfileEmployeeCommand ??= new(obj => { BackToProfileEmployee(); }); }

        private RelayCommand _selectedImageCommand;
        public RelayCommand SelectedImageCommand { get => _selectedImageCommand ??= new(obj => { SelectedImage(); }); }

        private RelayCommand _clearImageCommand;
        public RelayCommand ClearImageCommand { get => _clearImageCommand ??= new(obj => { ImageDocumentEmployees.Clear(); }); }

        private RelayCommand _deleteOneImageCommand;
        public RelayCommand DeleteOneImageCommand => _deleteOneImageCommand ??= new RelayCommand(DeleteOneImage);

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------------------------
        #region Методы

        private void AddDocumentEmployee()
        {
            Document document = new Document()
            {
                IdEmployee = CurrentUser.IdEmployee,
                IdTypeDocument = SelectedTypeDocument.IdTypeDocument,
                Number = NumberDocumentTB.ToString(),
                PlaceOfIssue = PlaceOfIssueTB,
                //DateOfIssue = DateOfIssueDP.Value.Date,
                //DocumentImage = ImageDocumentEmployees.Where(x => x.Image == SelectImage),
                //DocumentImage = SelectImage,
                //DocumentFile = ImageDocumentEmployees.Where(x => x.NameImage == SelectImage),
            };
        }

        private void UpdateDocumentEmployee()
        {

        }

        private void BackToProfileEmployee()
        {
            _navigationPage.NavigateTo("ProfileEmployee", CurrentUser);
        }

        private void SelectedImage()
        {
            var files = _openFileDialogService.OpenDialog();

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

        private async void GetDocument()
        {
            Documents.Clear();

            var document = await _dataBaseService.GetDataTableAsync<Document>(x => x.Where(x => x.IdEmployee == CurrentUser.IdEmployee).Include(x => x.IdTypeDocumentNavigation));

            foreach (var item in document)
            {
                Documents.Add(item);
            }
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

        #endregion
    }
}
