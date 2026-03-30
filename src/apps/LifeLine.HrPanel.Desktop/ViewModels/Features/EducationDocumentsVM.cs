using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class EducationDocumentsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        private readonly IReadOnlyCollection<DocumentTypeDisplay> _documentTypes;
        private readonly IReadOnlyCollection<EducationLevelDisplay> _educationLevels;

        public EducationDocumentsVM
            (
                IFileDialogService fileDialogService, 
                IReadOnlyCollection<DocumentTypeDisplay> documentTypes,
                IReadOnlyCollection<EducationLevelDisplay> educationLevels
            )
        {
            _fileDialogService = fileDialogService;

            _documentTypes = documentTypes;
            _educationLevels = educationLevels;

            //CreateNewEducationDocument();

            SelectCommand = new RelayCommand(Execute_SelectCommand);
            AddEducationDocumentCommand = new RelayCommand(Execute_AddEducationDocumentCommand, CanExecute_AddEducationDocumentCommand);
            DeleteEducationDocumentCommand = new RelayCommand<EducationDocumentDisplay>(Execute_DeleteEducationDocumentCommand);
        }

        private string _documentNumber = null!;
        public string DocumentNumber
        {
            get => _documentNumber;
            set
            {
                SetProperty(ref _documentNumber, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _issuedDate;
        public DateTime IssuedDate
        {
            get => _issuedDate;
            set
            {
                SetProperty(ref _issuedDate, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _organizationName = null!;
        public string OrganizationName
        {
            get => _organizationName;
            set
            {
                SetProperty(ref _organizationName, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _qualificationAwardedName;
        public string? QualificationAwardedName
        {
            get => _qualificationAwardedName;
            set
            {
                SetProperty(ref _qualificationAwardedName, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _specialtyName;
        public string? SpecialtyName
        {
            get => _specialtyName;
            set
            {
                SetProperty(ref _specialtyName, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _programName;
        public string? ProgramName
        {
            get => _programName;
            set
            {
                SetProperty(ref _programName, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private TimeSpan? _totalHours;
        public TimeSpan? TotalHours
        {
            get => _totalHours;
            set
            {
                SetProperty(ref _totalHours, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private EducationLevelDisplay _educationLevel = null!;
        public EducationLevelDisplay EducationLevel
        {
            get => _educationLevel;
            set
            {
                SetProperty(ref _educationLevel, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private DocumentTypeDisplay _documentType = null!;
        public DocumentTypeDisplay DocumentType
        {
            get => _documentType;
            set
            {
                SetProperty(ref _documentType, value);
                AddEducationDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public RelayCommand SelectCommand { get; private set; }
        private void Execute_SelectCommand()
            => FilePath = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.ASSIGNMENT}", FileFilters.Images);

        private EducationDocumentDisplay _newEducationDocument = null!;
        private void CreateNewEducationDocument()
            => _newEducationDocument = new
                (
                    new EducationDocumentResponse
                        (
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            DateTime.UtcNow.ToString(), 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, "0"
                        ), [], [], string.Empty
                );

        public void ClearProperty()
        {
            DocumentNumber = string.Empty;
            IssuedDate = DateTime.UtcNow;
            OrganizationName = string.Empty;
            QualificationAwardedName = string.Empty;
            SpecialtyName = string.Empty;
            ProgramName = string.Empty;
            TotalHours = TimeSpan.Zero;
            EducationLevel = null!;
            DocumentType = null!;
            FilePath = string.Empty;
        }

        private EducationDocumentDisplay _selectedEducationDocumentDisplay = null!;
        public EducationDocumentDisplay SelectedEducationDocumentDisplay
        {
            get => _selectedEducationDocumentDisplay;
            set => SetProperty(ref _selectedEducationDocumentDisplay, value);
        }

        public ObservableCollection<EducationDocumentDisplay> LocalEducationDocuments { get; private init; } = [];

        public RelayCommand AddEducationDocumentCommand { get; private set; }
        private void Execute_AddEducationDocumentCommand()
        {
            //_newEducationDocument.DocumentNumber = DocumentNumber;
            //_newEducationDocument.IssuedDate = IssuedDate;
            //_newEducationDocument.OrganizationName = OrganizationName;
            //_newEducationDocument.QualificationAwardedName = QualificationAwardedName;
            //_newEducationDocument.SpecialtyName = SpecialtyName;
            //_newEducationDocument.ProgramName = ProgramName;
            //_newEducationDocument.TotalHours = TotalHours;
            //_newEducationDocument.EducationLevel = EducationLevel;
            //_newEducationDocument.DocumentType = DocumentType;
            //_newEducationDocument.FilePath = FilePath;

            LocalEducationDocuments.Add
                (
                    new EducationDocumentDisplay
                        (
                            new EducationDocumentResponse
                                (
                                    string.Empty,
                                    EmployeeId,
                                    EducationLevel.Id,
                                    DocumentType.Id,
                                    DocumentNumber,
                                    IssuedDate.ToString(),
                                    OrganizationName,
                                    QualificationAwardedName,
                                    SpecialtyName,
                                    ProgramName,
                                    TotalHours.ToString()
                                ),
                            _educationLevels, _documentTypes, FilePath
                        )
                );

            //CreateNewEducationDocument();

            ClearProperty();
        }
        private bool CanExecute_AddEducationDocumentCommand()
            => EducationLevel != null && DocumentType != null &&
               !string.IsNullOrWhiteSpace(DocumentNumber) &&
               !string.IsNullOrWhiteSpace(IssuedDate.ToString()) &&
               IssuedDate != DateTime.MinValue &&
               !string.IsNullOrWhiteSpace(OrganizationName);

        public RelayCommand<EducationDocumentDisplay>? DeleteEducationDocumentCommand { get; private set; }
        private void Execute_DeleteEducationDocumentCommand(EducationDocumentDisplay display)
            => LocalEducationDocuments.Remove(display);
    }
}
