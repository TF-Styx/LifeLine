using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    internal sealed class EducationDocumentDisplay(EducationDocumentResponse model) : BaseViewModel
    {
        private readonly EducationDocumentResponse _model = model;

        public string EmployeeId => _model.EmployeeId;
        public string EducationLevelId => _model.EducationLevelId;
        public string DocumentTypeId => _model.DocumentTypeId;

        //DocumentNumber
        private string _documentNumber = model.DocumentNumber;
        public string DocumentNumber
        {
            get => _documentNumber;
            set => SetProperty(ref _documentNumber, value);
        }

        //IssuedDate
        private DateTime _issuedDate = DateTime.Parse(model.IssuedDate);
        public DateTime IssuedDate
        {
            get => _issuedDate;
            set => SetProperty(ref _issuedDate, value);
        }

        //OrganizationName
        private string _organizationName = model.OrganizationName;
        public string OrganizationName
        {
            get => _organizationName;
            set => SetProperty(ref _organizationName, value);
        }

        //QualificationAwardedName
        private string? _qualificationAwardedName = model.QualificationAwardedName;
        public string? QualificationAwardedName
        {
            get => _qualificationAwardedName;
            set => SetProperty(ref _qualificationAwardedName, value);
        }

        //SpecialtyName
        private string? _specialtyName = model.SpecialtyName;
        public string? SpecialtyName
        {
            get => _specialtyName;
            set => SetProperty(ref _specialtyName, value);
        }

        //ProgramName
        private string? _programName = model.ProgramName;
        public string? ProgramName
        {
            get => _programName;
            set => SetProperty(ref _programName, value);
        }

        //TotalHours
        private TimeSpan? _totalHours = TimeSpan.Parse(model.TotalHours!);
        public TimeSpan? TotalHours
        {
            get => _totalHours;
            set => SetProperty(ref _totalHours, value);
        }

        //SelectedEducationLevel
        private EducationLevelResponse _educationLevel = null!;
        public EducationLevelResponse EducationLevel
        {
            get => _educationLevel;
            set => SetProperty(ref _educationLevel, value);
        }

        //SelectedDocumentType
        private DocumentTypeResponse _documentType = null!;
        public DocumentTypeResponse DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value);
        }
    }
}
