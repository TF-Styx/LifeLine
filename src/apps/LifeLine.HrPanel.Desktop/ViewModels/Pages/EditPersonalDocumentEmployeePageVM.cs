using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.PersonalDocument;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class EditPersonalDocumentEmployeePageVM : BasePageViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly INavigationPage _navigationPage;
        private readonly IDocumentTypeReadOnlyService _documentTypeReadOnlyService;
        private readonly IPersonalDocumentApiServiceFactory _personalDocumentApiServiceFactory;

        private bool _isEditMode;

        public EditPersonalDocumentEmployeePageVM
            (
                INavigationPage navigationPage,
                IDocumentTypeReadOnlyService documentTypeReadOnlyService,
                IPersonalDocumentApiServiceFactory personalDocumentApiServiceFactory
            )
        {
            _navigationPage = navigationPage;
            _documentTypeReadOnlyService = documentTypeReadOnlyService;
            _personalDocumentApiServiceFactory = personalDocumentApiServiceFactory;

            UpdatePersonalDocumentEmployeeCommand = new RelayCommandAsync(Execute_UpdatePersonalDocumentEmployeeCommand, CanExecute_UpdatePersonalDocumentEmployeeCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize) 
                return;

            IsInitialize = false;

            await GetAllDocumentTypeAsync();

            CreateNewPersonalDocumentDisplay();

            if (PersonalDocumentDisplay != null && SelectedDocumentType == null && _isEditMode)
            {
                if (PersonalDocumentDisplay.DocumentType != null)
                    SelectedDocumentType = DocumentTypes.FirstOrDefault(x => x.Id == PersonalDocumentDisplay.DocumentType.Id);
            }

            IsInitialize = true;
        }

        // 2. Исправляем метод получения данных
        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            if (value is ValueTuple<EmployeeDetailsDisplay, PersonalDocumentDisplay?> tuple)
            {
                CurrentEmployeeDetails = tuple.Item1;
                var incomingDocument = tuple.Item2;

                if (incomingDocument != null)
                {
                    // === РЕЖИМ РЕДАКТИРОВАНИЯ ===
                    _isEditMode = true;

                    PersonalDocumentDisplay = incomingDocument;

                    if (DocumentTypes.Count > 0 && PersonalDocumentDisplay.DocumentType != null)
                        SelectedDocumentType = DocumentTypes.FirstOrDefault(x => x.Id == PersonalDocumentDisplay.DocumentType.Id);
                }
                else
                {
                    // === РЕЖИМ СОЗДАНИЯ ===
                    _isEditMode = false;

                    PersonalDocumentDisplay = new PersonalDocumentDisplay(new PersonalDocumentResponse(Guid.Empty, Guid.Empty, string.Empty, string.Empty), DocumentTypes);

                    SelectedDocumentType = null;
                }

                UpdatePersonalDocumentEmployeeCommand?.RaiseCanExecuteChanged();
            }
        }

        #region Display

        private EmployeeDetailsDisplay _currentEmployeeDetails = null!;
        public EmployeeDetailsDisplay CurrentEmployeeDetails
        {
            get => _currentEmployeeDetails;
            set => SetProperty(ref _currentEmployeeDetails, value);
        }

        private DocumentTypeDisplay? _selectedDocumentType;
        public DocumentTypeDisplay? SelectedDocumentType
        {
            get => _selectedDocumentType;
            set => SetProperty(ref _selectedDocumentType, value);
        }

        public ObservableCollection<DocumentTypeDisplay> DocumentTypes { get; private init; } = [];
        private async Task GetAllDocumentTypeAsync()
        {
            var documentTypes = await _documentTypeReadOnlyService.GetAllAsync();

            DocumentTypes.Load(documentTypes.Select(documentType => new DocumentTypeDisplay(documentType)).ToList());
        }

        private PersonalDocumentDisplay _personalDocumentDisplay = null!;
        public PersonalDocumentDisplay PersonalDocumentDisplay
        {
            get => _personalDocumentDisplay;
            set => SetProperty(ref _personalDocumentDisplay, value);
        }
        private void CreateNewPersonalDocumentDisplay()
            => PersonalDocumentDisplay = new PersonalDocumentDisplay(new PersonalDocumentResponse(Guid.Empty, Guid.Empty, string.Empty, string.Empty), DocumentTypes);

        #endregion

        public RelayCommandAsync UpdatePersonalDocumentEmployeeCommand { get; private set; }

        private async Task Execute_UpdatePersonalDocumentEmployeeCommand()
        {
            // Простейшая валидация
            if (SelectedDocumentType == null)
            {
                MessageBox.Show("Выберите тип документа");
                return;
            }

            if (_isEditMode)
            {
                // UPDATE
                var resultUpdate = await _personalDocumentApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdatePersonalDocumentAsync
                (
                    PersonalDocumentDisplay.PersonalDocumentId,
                    new UpdatePersonalDocumentRequest
                    (
                        SelectedDocumentType.Id,
                        PersonalDocumentDisplay.DocumentNumber,
                        PersonalDocumentDisplay.DocumentSeries
                    )
                );

                if (resultUpdate.IsSuccess)
                    _navigationPage.TransmittingValue(PersonalDocumentDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Update);
                else
                    MessageBox.Show($"Обновление персональных документов: {resultUpdate.StringMessage}");
            }
            else
            {
                // CREATE
                var resultCreate = await _personalDocumentApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).CreateAsync
                (
                    new CreatePersonalDocumentRequest
                    (
                        Guid.Parse(SelectedDocumentType.Id),
                        PersonalDocumentDisplay.DocumentNumber,
                        PersonalDocumentDisplay.DocumentSeries
                    )
                );

                if (resultCreate.IsSuccess)
                {
                    PersonalDocumentDisplay.SetDocumentType(SelectedDocumentType.Id);

                    _navigationPage.TransmittingValue(PersonalDocumentDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Create);
                }
                else
                    MessageBox.Show($"Внесение персональных документов: {resultCreate.StringMessage}");
            }
        }

        private bool CanExecute_UpdatePersonalDocumentEmployeeCommand() => true;
    }
}
