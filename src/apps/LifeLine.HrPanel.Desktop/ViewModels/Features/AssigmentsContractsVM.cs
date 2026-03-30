using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Extensions;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class AssigmentsContractsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IPositionReadOnlyApiServiceFactory _positionReadOnlyApiServiceFactory;

        private readonly IReadOnlyCollection<DepartmentDisplay> _departments;
        private readonly IReadOnlyCollection<ManagerDisplay> _managers;
        private readonly IReadOnlyCollection<StatusDisplay> _statuses;
        private readonly IReadOnlyCollection<EmployeeTypeDisplay> _employeeTypes;

        public AssigmentsContractsVM
            (
                IFileDialogService fileDialogService, 
                IPositionReadOnlyApiServiceFactory positionReadOnlyApiServiceFactory,

                IReadOnlyCollection<DepartmentDisplay> departments,
                IReadOnlyCollection<ManagerDisplay> managers,
                IReadOnlyCollection<StatusDisplay> statuses,
                IReadOnlyCollection<EmployeeTypeDisplay> employeeTypes
            )
        {
            _fileDialogService = fileDialogService;
            _positionReadOnlyApiServiceFactory = positionReadOnlyApiServiceFactory;

            _departments = departments;
            _managers = managers;
            _statuses = statuses;
            _employeeTypes = employeeTypes;

            //CreateNewAssignmentContract();

            SelectCommand = new RelayCommand(Execute_SelectCommand);
            AddAssignmentContractCommand = new RelayCommand(Execute_AddAssignmentContractCommand, CanExecute_AddAssignmentContractCommand);
            DeleteAssignmentContractCommand = new RelayCommand<AssignmentContractDisplay>(Execute_DeleteAssignmentContractCommand);

            _getAllPositionByIdDepartmentCommandAsync = new RelayCommandAsync<DepartmentDisplay>(Execute_GetAllPositionByIdDepartmentCommandAsyn);
        }

        #region Assignment

        private DepartmentDisplay _department = null!;
        public DepartmentDisplay Department
        {
            get => _department;
            set
            {
                if (SetProperty(ref _department, value))
                {
                    if (value != null)
                        _getAllPositionByIdDepartmentCommandAsync.Execute(value);
                    else
                        Positions.Clear();
                }

                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private PositionDisplay _position = null!;
        public PositionDisplay Position
        {
            get => _position;
            set
            {
                SetProperty(ref _position, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private ManagerDisplay? _manager;
        public ManagerDisplay? Manager
        {
            get => _manager;
            set
            {
                SetProperty(ref _manager, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _hireDate = DateTime.Now;
        public DateTime HireDate
        {
            get => _hireDate;
            set
            {
                SetProperty(ref _hireDate, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _terminationDate = DateTime.Now;
        public DateTime? TerminationDate
        {
            get => _terminationDate;
            set
            {
                SetProperty(ref _terminationDate, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private StatusDisplay _status = null!;
        public StatusDisplay Status
        {
            get => _status;
            set
            {
                SetProperty(ref _status, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
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

        #endregion

        #region Contract

        private EmployeeTypeDisplay _employeeType = null!;
        public EmployeeTypeDisplay EmployeeType
        {
            get => _employeeType;
            set
            {
                SetProperty(ref _employeeType, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _contractNumber = null!;
        public string ContractNumber
        {
            get => _contractNumber;
            set
            {
                SetProperty(ref _contractNumber, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        private decimal _salary;
        public decimal Salary
        {
            get => _salary;
            set
            {
                SetProperty(ref _salary, value);
                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        private AssignmentContractDisplay? _newAssignmentContract;
        private void CreateNewAssignmentContract()
            => _newAssignmentContract = new
                (
                    new AssignmentResponse
                        (
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            DateTime.UtcNow,
                            DateTime.UtcNow,
                            string.Empty
                        ),
                    new ContractResponse
                        (
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            DateTime.UtcNow,
                            DateTime.UtcNow,
                            decimal.Zero,
                            string.Empty
                        ),
                    [], [], [], [], [], string.Empty
                );

        public void ClearProperty()
        {
            Department = null!;
            Position = null!;
            Manager = null;
            HireDate = HireDate;
            Status = null!;
            FilePath = string.Empty;
            
            EmployeeType = null!;
            ContractNumber = string.Empty;
            StartDate = DateTime.UtcNow;
            EndDate = DateTime.UtcNow;
            Salary = decimal.Zero;
        }

        public ObservableCollection<PositionDisplay> Positions { get; private init; } = [];
        private RelayCommandAsync<DepartmentDisplay> _getAllPositionByIdDepartmentCommandAsync;
        private async Task Execute_GetAllPositionByIdDepartmentCommandAsyn(DepartmentDisplay display)
        {
            if (display == null || display.Id == string.Empty)
            {
                Positions.Clear();
                return;
            }

            var positions = await _positionReadOnlyApiServiceFactory.Create(display.Id.ToString()).GetAllAsync();

            Positions.Load([.. positions.Select(position => new PositionDisplay(position))], cleaning: true);
        }

        public ObservableCollection<AssignmentContractDisplay> LocalAssignmentsContracts { get; private init; } = [];

        public RelayCommand AddAssignmentContractCommand { get; private set; }
        private void Execute_AddAssignmentContractCommand()
        {
            //_newAssignmentContract.Department = Department;
            //_newAssignmentContract.Position = Position;
            //_newAssignmentContract.Manager = Manager;
            //_newAssignmentContract.HireDate = HireDate;
            //_newAssignmentContract.Status = Status;
            //_newAssignmentContract.FilePath = FilePath;

            //_newAssignmentContract.EmployeeType = EmployeeType;
            //_newAssignmentContract.ContractNumber = ContractNumber;
            //_newAssignmentContract.StartDate = StartDate;
            //_newAssignmentContract.EndDate = EndDate;
            //_newAssignmentContract.Salary = Salary;

            LocalAssignmentsContracts.Add
                (
                    new AssignmentContractDisplay
                        (
                            new AssignmentResponse
                                (
                                    string.Empty,
                                    EmployeeId,
                                    Position.Id,
                                    Department.Id,
                                    Manager?.Id,
                                    HireDate,
                                    TerminationDate,
                                    Status.Id
                                ),
                            new ContractResponse
                                (
                                    EmployeeId,
                                    string.Empty,
                                    ContractNumber,
                                    EmployeeType.Id,
                                    StartDate,
                                    EndDate,
                                    Salary,
                                    null
                                ),
                            _departments, Positions, _managers, _statuses, _employeeTypes, FilePath
                        )
                );

            //CreateNewAssignmentContract();

            ClearProperty();
        }
        private bool CanExecute_AddAssignmentContractCommand()
            => Department != null && Position != null &&
               EmployeeType != null && Status != null &&
               !string.IsNullOrWhiteSpace(HireDate.ToString()) &&
               !string.IsNullOrWhiteSpace(ContractNumber) &&
               !string.IsNullOrWhiteSpace(StartDate.ToString()) &&
               !string.IsNullOrWhiteSpace(EndDate.ToString()) &&
               !string.IsNullOrWhiteSpace(Salary.ToString());

        public RelayCommand<AssignmentContractDisplay> DeleteAssignmentContractCommand { get; private set; }
        private void Execute_DeleteAssignmentContractCommand(AssignmentContractDisplay display)
            => LocalAssignmentsContracts.Remove(display);
    }
}
