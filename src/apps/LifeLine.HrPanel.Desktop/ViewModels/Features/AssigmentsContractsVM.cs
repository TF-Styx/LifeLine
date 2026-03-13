using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class AssigmentsContractsVM : BaseViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        public AssigmentsContractsVM(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;

            SelectCommand = new RelayCommand(Execute_SelectCommand);
        }

        #region Assignment

        private DepartmentResponse _department = null!;
        public DepartmentResponse Department
        {
            get => _department;
            set => SetProperty(ref _department, value);
        }

        private PositionResponse _position = null!;
        public PositionResponse Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private ManagerEmployeeResponse? _manager;
        public ManagerEmployeeResponse? Manager
        {
            get => _manager;
            set => SetProperty(ref _manager, value);
        }

        private DateTime _hireDate;
        public DateTime HireDate
        {
            get => _hireDate;
            set => SetProperty(ref _hireDate, value);
        }

        private DateTime? _terminationDate;
        public DateTime? TerminationDate
        {
            get => _terminationDate;
            set => SetProperty(ref _terminationDate, value);
        }

        private StatusResponse _status = null!;
        public StatusResponse Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public RelayCommand SelectCommand { get; private set; }
        private void Execute_SelectCommand()
            => FilePath = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.ASSIGNMENT}", FileFilters.Images);

        #endregion

        #region Contract

        private EmployeeTypeResponse _employeeType = null!;
        public EmployeeTypeResponse EmployeeType
        {
            get => _employeeType;
            set => SetProperty(ref _employeeType, value);
        }

        private string _contractNumber;
        public string ContractNumber
        {
            get => _contractNumber;
            set => SetProperty(ref _contractNumber, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        private decimal _salary;
        public decimal Salary
        {
            get => _salary;
            set => SetProperty(ref _salary, value);
        }

        #endregion
    }
}
