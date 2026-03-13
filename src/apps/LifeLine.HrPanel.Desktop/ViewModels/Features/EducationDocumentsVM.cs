using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class EducationDocumentsVM : BaseViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        public EducationDocumentsVM(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;

            SelectCommand = new RelayCommand(Execute_SelectCommand);
        }

        private string _documentNumber = null!;
        public string DocumentNumber
        {
            get => _documentNumber;
            set => SetProperty(ref _documentNumber, value);
        }

        private DateTime _issuedDate;
        public DateTime IssuedDate
        {
            get => _issuedDate;
            set => SetProperty(ref _issuedDate, value);
        }

        private string _organizationName = null!;
        public string OrganizationName
        {
            get => _organizationName;
            set => SetProperty(ref _organizationName, value);
        }

        private string? _qualificationAwardedName;
        public string? QualificationAwardedName
        {
            get => _qualificationAwardedName;
            set => SetProperty(ref _qualificationAwardedName, value);
        }

        private string? _specialtyName;
        public string? SpecialtyName
        {
            get => _specialtyName;
            set => SetProperty(ref _specialtyName, value);
        }

        private string? _programName;
        public string? ProgramName
        {
            get => _programName;
            set => SetProperty(ref _programName, value);
        }

        private TimeSpan? _totalHours;
        public TimeSpan? TotalHours
        {
            get => _totalHours;
            set => SetProperty(ref _totalHours, value);
        }

        private EducationLevelResponse _educationLevel = null!;
        public EducationLevelResponse EducationLevel
        {
            get => _educationLevel;
            set => SetProperty(ref _educationLevel, value);
        }

        private DocumentTypeResponse _documentType = null!;
        public DocumentTypeResponse DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value);
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public RelayCommand SelectCommand { get; private set; }
        private void Execute_SelectCommand()
            => FilePath = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.ASSIGNMENT}", FileFilters.Images);
    }
}
