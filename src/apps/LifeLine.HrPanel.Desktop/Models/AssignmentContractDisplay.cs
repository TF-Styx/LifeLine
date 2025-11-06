using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class AssignmentContractDisplay : BaseViewModel
    {
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

        private EmployeeResponse? _manager;
        public EmployeeResponse? Manager
        {
            get => _manager;
            set => SetProperty(ref _manager, value);
        }

        private DateTime _hireDate = DateTime.Now;
        public DateTime HireDate
        {
            get => _hireDate;
            set => SetProperty(ref _hireDate, value);
        }

        private DateTime _terminationDate = DateTime.Now;
        public DateTime TerminationDate
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

        #endregion

        #region Contract

        private EmployeeTypeResponse _employeeType = null!;
        public EmployeeTypeResponse EmployeeType
        {
            get => _employeeType;
            set => SetProperty(ref _employeeType, value);
        }

        private string _contractNumber = null!;
        public string ContractNumber
        {
            get => _contractNumber;
            set => SetProperty(ref _contractNumber, value);
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        private decimal _salary = decimal.Zero;
        public decimal Salary
        {
            get => _salary;
            set => SetProperty(ref _salary, value);
        }

        #endregion
    }
}
