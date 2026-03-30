using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class WorkPermitsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        private readonly IReadOnlyCollection<PermitTypeDisplay> _permitTypes;
        private readonly IReadOnlyCollection<AdmissionStatusDisplay> _admissionStatuses;

        public WorkPermitsVM
            (
                IFileDialogService fileDialogService, 
                IReadOnlyCollection<PermitTypeDisplay> permitTypes, 
                IReadOnlyCollection<AdmissionStatusDisplay> admissionStatuses
            )
        {
            _fileDialogService = fileDialogService;

            _permitTypes = permitTypes;
            _admissionStatuses = admissionStatuses;

            //CreateNewWorkPermit();

            SelectCommand = new RelayCommand(Execute_SelectCommand);
            AddWorkPermitCommand = new RelayCommand(Execute_AddWorkPermitCommand, CanExecute_AddWorkPermitCommand);
            DeleteWorkPermitCommand = new RelayCommand<WorkPermitDisplay>(Execute_DeleteWorkPermitCommand);
            _permitTypes = permitTypes;
            _admissionStatuses = admissionStatuses;
        }

        private string _workPermitName = null!;
        public string WorkPermitName
        {
            get => _workPermitName;
            set
            {
                SetProperty(ref _workPermitName, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _documentSeries;
        public string? DocumentSeries
        {
            get => _documentSeries;
            set
            {
                SetProperty(ref _documentSeries, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _workPermitNumber = null!;
        public string WorkPermitNumber
        {
            get => _workPermitNumber;
            set
            {
                SetProperty(ref _workPermitNumber, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _protocolNumber;
        public string? ProtocolNumber
        {
            get => _protocolNumber;
            set
            {
                SetProperty(ref _protocolNumber, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _specialtyName = null!;
        public string SpecialtyName
        {
            get => _specialtyName;
            set
            {
                SetProperty(ref _specialtyName, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _issuingAuthority = null!;
        public string IssuingAuthority
        {
            get => _issuingAuthority;
            set
            {
                SetProperty(ref _issuingAuthority, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _issueDate;
        public DateTime IssueDate
        {
            get => _issueDate;
            set
            {
                SetProperty(ref _issueDate, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _expiryDate;
        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set
            {
                SetProperty(ref _expiryDate, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private PermitTypeDisplay _permitType = null!;
        public PermitTypeDisplay PermitType
        {
            get => _permitType;
            set
            {
                SetProperty(ref _permitType, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        private AdmissionStatusDisplay _admissionStatus = null!;
        public AdmissionStatusDisplay AdmissionStatus
        {
            get => _admissionStatus;
            set
            {
                SetProperty(ref _admissionStatus, value);
                AddWorkPermitCommand?.RaiseCanExecuteChanged();
            }
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public RelayCommand SelectCommand { get; private set; }
        private void Execute_SelectCommand()
            => FilePath = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.WORK_PERMIT}", FileFilters.Images);

        private WorkPermitDisplay _newWorkPermit = null!;
        private void CreateNewWorkPermit()
            => _newWorkPermit = new
                (
                    new WorkPermitResponse
                        (
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            DateTime.UtcNow, 
                            DateTime.UtcNow, 
                            string.Empty, 
                            string.Empty
                        ), [], [], string.Empty
                );

        public void ClearProperty()
        {
            WorkPermitName = string.Empty;
            DocumentSeries = string.Empty;
            WorkPermitNumber = string.Empty;
            ProtocolNumber = string.Empty;
            SpecialtyName = string.Empty;
            IssuingAuthority = string.Empty;
            IssueDate = DateTime.UtcNow;
            ExpiryDate = DateTime.UtcNow;
            PermitType = null!;
            AdmissionStatus = null!;
            FilePath = string.Empty;
        }

        private WorkPermitDisplay _selectedWorkPermit = null!;
        public WorkPermitDisplay SelectedWorkPermit
        {
            get => _selectedWorkPermit;
            set => SetProperty(ref _selectedWorkPermit, value);
        }

        public ObservableCollection<WorkPermitDisplay> LocalWorkPermits { get; private init; } = [];

        public RelayCommand AddWorkPermitCommand { get; private set; }
        private void Execute_AddWorkPermitCommand()
        {
            //_newWorkPermit.WorkPermitName = WorkPermitName;
            //_newWorkPermit.DocumentSeries = DocumentSeries;
            //_newWorkPermit.WorkPermitNumber = WorkPermitNumber;
            //_newWorkPermit.ProtocolNumber = ProtocolNumber;
            //_newWorkPermit.SpecialtyName = SpecialtyName;
            //_newWorkPermit.IssuingAuthority = IssuingAuthority;
            //_newWorkPermit.IssueDate = IssueDate;
            //_newWorkPermit.ExpiryDate = ExpiryDate;
            //_newWorkPermit.PermitType = PermitType;
            //_newWorkPermit.AdmissionStatus = AdmissionStatus;
            //_newWorkPermit.FilePath = FilePath;

            LocalWorkPermits.Add
                (
                    new WorkPermitDisplay
                        (
                            new WorkPermitResponse
                                (
                                    string.Empty,
                                    EmployeeId,
                                    WorkPermitName,
                                    DocumentSeries,
                                    WorkPermitNumber,
                                    ProtocolNumber,
                                    SpecialtyName,
                                    IssuingAuthority,
                                    IssueDate,
                                    ExpiryDate,
                                    PermitType.Id,
                                    AdmissionStatus.Id
                                ),
                            _permitTypes, _admissionStatuses, FilePath
                        )
                );

            //CreateNewWorkPermit();

            ClearProperty();
        }
        private bool CanExecute_AddWorkPermitCommand()
            => AdmissionStatus != null && PermitType != null &&
            !string.IsNullOrWhiteSpace(WorkPermitName) &&
            !string.IsNullOrWhiteSpace(WorkPermitNumber) &&
            !string.IsNullOrWhiteSpace(SpecialtyName) &&
            !string.IsNullOrWhiteSpace(IssuingAuthority) &&
            IssueDate != DateTime.MinValue && 
            !string.IsNullOrWhiteSpace(IssueDate.ToString()) &&
            ExpiryDate != DateTime.MinValue &&
            !string.IsNullOrWhiteSpace(ExpiryDate.ToString());

        public RelayCommand<WorkPermitDisplay> DeleteWorkPermitCommand { get; private set; }
        private void Execute_DeleteWorkPermitCommand(WorkPermitDisplay display)
            => LocalWorkPermits.Remove(display);
    }
}
