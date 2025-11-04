using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class WorkPermitDisplay(WorkPermitResponse model) : BaseViewModel
    {
        private readonly WorkPermitResponse _model = model;

        public string EmployeeId => _model.EmployeeId;
        public string PermitTypeId => _model.PermitTypeId;
        public string AdmissionStatusId => _model.AdmissionStatusId;

        //WorkPermitName
        private string _workPermitName = model.WorkPermitName;
        public string WorkPermitName
        {
            get => _workPermitName;
            set => SetProperty(ref _workPermitName, value);
        }

        //DocumentSeries
        private string? _documentSeries = model.DocumentSeries;
        public string? DocumentSeries
        {
            get => _documentSeries;
            set => SetProperty(ref _documentSeries, value);
        }

        //WorkPermitNumber
        private string _workPermitNumber = model.WorkPermitNumber;
        public string WorkPermitNumber
        {
            get => _workPermitNumber;
            set => SetProperty(ref _workPermitNumber, value);
        }

        //ProtocolNumber
        private string? _protocolNumber = model.ProtocolNumber;
        public string? ProtocolNumber
        {
            get => _protocolNumber;
            set => SetProperty(ref _protocolNumber, value);
        }

        //SpecialtyName
        private string _specialtyName = model.SpecialtyName;
        public string SpecialtyName
        {
            get => _specialtyName;
            set => SetProperty(ref _specialtyName, value);
        }

        //IssuingAuthority
        private string _issuingAuthority = model.IssuingAuthority;
        public string IssuingAuthority
        {
            get => _issuingAuthority;
            set => SetProperty(ref _issuingAuthority, value);
        }

        //IssueDate
        private DateTime _issueDate = model.IssueDate;
        public DateTime IssueDate
        {
            get => _issueDate;
            set => SetProperty(ref _issueDate, value);
        }

        //ExpiryDate
        private DateTime _expiryDate = model.ExpiryDate;
        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set => SetProperty(ref _expiryDate, value);
        }

        //SelectedPermitType
        private AdmissionStatusResponse _permitType = null!;
        public AdmissionStatusResponse PermitType
        {
            get => _permitType;
            set => SetProperty(ref _permitType, value);
        }

        //SelectedAdmissionStatus
        private AdmissionStatusResponse _admissionStatus = null!;
        public AdmissionStatusResponse AdmissionStatus
        {
            get => _admissionStatus;
            set => SetProperty(ref _admissionStatus, value);
        }
    }
}
