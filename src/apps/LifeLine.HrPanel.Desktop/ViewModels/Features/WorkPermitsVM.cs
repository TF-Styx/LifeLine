using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class WorkPermitsVM : BaseViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        public WorkPermitsVM(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;

            SelectCommand = new RelayCommand(Execute_SelectCommand);
        }

        private string _workPermitName = null!;
        public string WorkPermitName
        {
            get => _workPermitName;
            set => SetProperty(ref _workPermitName, value);
        }

        private string? _documentSeries;
        public string? DocumentSeries
        {
            get => _documentSeries;
            set => SetProperty(ref _documentSeries, value);
        }

        private string _workPermitNumber = null!;
        public string WorkPermitNumber
        {
            get => _workPermitNumber;
            set => SetProperty(ref _workPermitNumber, value);
        }

        private string? _protocolNumber;
        public string? ProtocolNumber
        {
            get => _protocolNumber;
            set => SetProperty(ref _protocolNumber, value);
        }

        private string _specialtyName = null!;
        public string SpecialtyName
        {
            get => _specialtyName;
            set => SetProperty(ref _specialtyName, value);
        }

        private string _issuingAuthority = null!;
        public string IssuingAuthority
        {
            get => _issuingAuthority;
            set => SetProperty(ref _issuingAuthority, value);
        }

        private DateTime _issueDate;
        public DateTime IssueDate
        {
            get => _issueDate;
            set => SetProperty(ref _issueDate, value);
        }

        private DateTime _expiryDate;
        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set => SetProperty(ref _expiryDate, value);
        }

        private PermitTypeResponse _permitType = null!;
        public PermitTypeResponse PermitType
        {
            get => _permitType;
            set => SetProperty(ref _permitType, value);
        }

        private AdmissionStatusResponse _admissionStatus = null!;
        public AdmissionStatusResponse AdmissionStatus
        {
            get => _admissionStatus;
            set => SetProperty(ref _admissionStatus, value);
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public RelayCommand SelectCommand { get; private set; }
        private void Execute_SelectCommand()
            => FilePath = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.WORK_PERMIT}", FileFilters.Images);
    }
}
