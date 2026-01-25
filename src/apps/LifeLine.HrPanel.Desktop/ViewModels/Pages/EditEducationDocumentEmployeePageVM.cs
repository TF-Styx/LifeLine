using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Directory.Service.Client.Services.EducationLevel;
using LifeLine.Employee.Service.Client.Services.Employee.EducationDocument;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.EducationDocument;
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
    public sealed class EditEducationDocumentEmployeePageVM : BasePageViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly INavigationPage _navigationPage;

        private readonly IDocumentTypeReadOnlyService _documentTypeReadOnlyService;
        private readonly IEducationLevelReadOnlyService _educationLevelReadOnlyService;
        private readonly IEducationDocumentApiServiceFactory _educationDocumentApiServiceFactory;

        private bool _isEditMode;

        public EditEducationDocumentEmployeePageVM
            (
                INavigationPage navigationPage,

                IDocumentTypeReadOnlyService documentTypeReadOnlyService,
                IEducationLevelReadOnlyService educationLevelReadOnlyService,
                IEducationDocumentApiServiceFactory educationDocumentApiServiceFactory
            )
        {
            _navigationPage = navigationPage;

            _documentTypeReadOnlyService = documentTypeReadOnlyService;
            _educationLevelReadOnlyService = educationLevelReadOnlyService;
            _educationDocumentApiServiceFactory = educationDocumentApiServiceFactory;

            UpdateEducationDocumentEmployeeCommand = new RelayCommandAsync(Execute_UpdateEducationDocumentEmployeeCommand, CanExecute_UpdateEducationDocumentEmployeeCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllDocumentTypeAsync();
            await GetAllEducationLevelsAsync();

            CreateNewEducationDocumentDisplay();

            if (EducationDocumentDisplay != null && DocumentTypes.Count > 0 && EducationLevels.Count > 0)
            {
                if (EducationDocumentDisplay.DocumentType != null)
                    SelectedDocumentType = DocumentTypes.FirstOrDefault(x => x.Id == EducationDocumentDisplay.DocumentType.Id)!;

                if (EducationDocumentDisplay.EducationLevel != null)
                    SelectedEducationLevel = EducationLevels.FirstOrDefault(x => x.Id == EducationDocumentDisplay.EducationLevel.Id)!;
            }

            IsInitialize = true;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            if (value is ValueTuple<EmployeeDetailsDisplay, EducationDocumentDisplay> tuple)
            {
                CurrentEmployeeDetails = tuple.Item1;
                var incomingDocument = tuple.Item2;

                if (incomingDocument != null)
                {
                    _isEditMode = true;

                    EducationDocumentDisplay = incomingDocument;

                    if (EducationDocumentDisplay != null && DocumentTypes.Count > 0 && EducationLevels.Count > 0)
                    {
                        SelectedDocumentType = DocumentTypes.FirstOrDefault(x => x.Id == EducationDocumentDisplay.DocumentType.Id)!;
                        SelectedEducationLevel = EducationLevels.FirstOrDefault(x => x.Id == EducationDocumentDisplay.EducationLevel.Id)!;
                    }
                }
                else
                {
                    _isEditMode = false;

                    CreateNewEducationDocumentDisplay();

                    SelectedDocumentType = null;
                    SelectedEducationLevel = null;
                }

                UpdateEducationDocumentEmployeeCommand?.RaiseCanExecuteChanged();
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

        private EducationLevelDisplay? _selectedEducationLevel;
        public EducationLevelDisplay? SelectedEducationLevel
        {
            get => _selectedEducationLevel;
            set => SetProperty(ref _selectedEducationLevel, value);
        }

        public ObservableCollection<EducationLevelDisplay> EducationLevels { get; private init; } = [];
        private async Task GetAllEducationLevelsAsync()
        {
            var educationLevels = await _educationLevelReadOnlyService.GetAllAsync();

            EducationLevels.Load(educationLevels.Select(educationLevel => new EducationLevelDisplay(educationLevel)).ToList());
        }

        private EducationDocumentDisplay _educationDocumentDisplay = null!;
        public EducationDocumentDisplay EducationDocumentDisplay
        {
            get => _educationDocumentDisplay;
            set => SetProperty(ref _educationDocumentDisplay, value);
        }
        private void CreateNewEducationDocumentDisplay()
            => EducationDocumentDisplay = new EducationDocumentDisplay
                   (
                       new EducationDocumentResponse
                           (
                               string.Empty,
                               string.Empty,
                               string.Empty,
                               string.Empty,
                               string.Empty,
                               DateTime.Now.ToString(),
                               string.Empty,
                               string.Empty,
                               string.Empty,
                               string.Empty,
                               TimeSpan.Zero.ToString()
                           ),
                       EducationLevels,
                       DocumentTypes
                   );

        #endregion

        public RelayCommandAsync UpdateEducationDocumentEmployeeCommand { get; private set; }
        public async Task Execute_UpdateEducationDocumentEmployeeCommand()
        {
            if (_isEditMode)
            {
                var resultUpdate = await _educationDocumentApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdateEducationDocumentAsync
                (
                    Guid.Parse(EducationDocumentDisplay.EducationDocumentId),
                    new UpdateEducationDocumentRequest
                        (
                            SelectedEducationLevel!.Id,
                            SelectedDocumentType!.Id,
                            EducationDocumentDisplay.DocumentNumber,
                            EducationDocumentDisplay.IssuedDate,
                            EducationDocumentDisplay.OrganizationName,
                            EducationDocumentDisplay.QualificationAwardedName,
                            EducationDocumentDisplay.SpecialtyName,
                            EducationDocumentDisplay.ProgramName,
                            EducationDocumentDisplay.TotalHours
                        )
                );

                if (resultUpdate.IsSuccess)
                    _navigationPage.TransmittingValue(EducationDocumentDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Update);
                else
                    MessageBox.Show($"Обновление послеучебных документов: {resultUpdate.StringMessage}");
            }
            else
            {
                double totalHours = 0;
                if (EducationDocumentDisplay.TotalHours != null)
                    totalHours = EducationDocumentDisplay.TotalHours.Value.TotalHours;

                var resultCreate = await _educationDocumentApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).CreateAsync
                    (
                        new CreateEducationDocumentRequest
                            (
                                Guid.Parse(SelectedEducationLevel!.Id),
                                Guid.Parse(SelectedDocumentType!.Id),
                                EducationDocumentDisplay.DocumentNumber,
                                EducationDocumentDisplay.IssuedDate,
                                EducationDocumentDisplay.OrganizationName,
                                EducationDocumentDisplay.QualificationAwardedName,
                                EducationDocumentDisplay.SpecialtyName,
                                EducationDocumentDisplay.ProgramName,
                                totalHours
                            )
                    );

                if (resultCreate.IsSuccess)
                {
                    EducationDocumentDisplay.SetEducationLevel(SelectedEducationLevel.Id);
                    EducationDocumentDisplay.SetDocumentType(SelectedDocumentType.Id);

                    _navigationPage.TransmittingValue(EducationDocumentDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Create);
                }
                else
                    MessageBox.Show($"Внесение послеучебных документов: {resultCreate.StringMessage}");
            }
        }
        private bool CanExecute_UpdateEducationDocumentEmployeeCommand() => true; /*SelectedEducationLevel != null && SelectedDocumentType != null;*/
    }
}