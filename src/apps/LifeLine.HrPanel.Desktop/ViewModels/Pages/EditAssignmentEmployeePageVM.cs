using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.Directory.Service.Client.Services.Status;
using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.Employee.Service.Client.Services.Employee.Assignment;
using LifeLine.Employee.Service.Client.Services.EmployeeType;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.Assignment;
using Shared.Contracts.Request.EmployeeService.WorkPermit;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class EditAssignmentEmployeePageVM : BaseViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly INavigationPage _navigationPage;

        private readonly IEmployeeService _employeeService;
        private readonly IStatusReadOnlyService _statusReadOnlyService;
        private readonly IDepartmentReadOnlyService _departmentReadOnlyService;
        private readonly IAssignmentApiServiceFactory _assignmentApiServiceFactory;
        private readonly IEmployeeTypeReadOnlyService _employeeTypeReadOnlyService;
        private readonly IPositionReadOnlyApiServiceFactory _positionReadOnlyApiServiceFactory;

        public EditAssignmentEmployeePageVM
            (
                INavigationPage navigationPage,

                IEmployeeService employeeService,
                IStatusReadOnlyService statusReadOnlyService,
                IDepartmentReadOnlyService departmentReadOnlyService,
                IAssignmentApiServiceFactory assignmentApiServiceFactory,
                IEmployeeTypeReadOnlyService employeeTypeReadOnlyService,
                IPositionReadOnlyApiServiceFactory positionReadOnlyApiServiceFactory
            )
        {
            _navigationPage = navigationPage;

            _employeeService = employeeService;
            _statusReadOnlyService = statusReadOnlyService;
            _departmentReadOnlyService = departmentReadOnlyService;
            _assignmentApiServiceFactory = assignmentApiServiceFactory;
            _employeeTypeReadOnlyService = employeeTypeReadOnlyService;
            _positionReadOnlyApiServiceFactory = positionReadOnlyApiServiceFactory;

            UpdateAssignmentEmployeeCommand = new RelayCommandAsync(Execute_UpdateAssignmentEmployeeCommand, CanExecute_UpdateAssignmentEmployeeCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllDepartmentAsync();
            await GetAllManagerAsync();
            await GetAllStatusAsync();
            await GetAllEmployeeTypeAsync();

            if (AssignmentContractDisplay != null && Departments.Count > 0 && Statuses.Count > 0 && EmployeeTypes.Count > 0)
            {
                AssignmentContractDisplay.Department = Departments.FirstOrDefault(x => x.Id == AssignmentContractDisplay.Department.Id)!;
                await GetAllPositionByIdDepartmentAsync();
                AssignmentContractDisplay.Position = Positions.FirstOrDefault(x => x.Id == AssignmentContractDisplay.Position.Id)!;
                AssignmentContractDisplay.Manager = Managers.FirstOrDefault(x => x.Id == AssignmentContractDisplay.Manager?.Id);
                AssignmentContractDisplay.Status = Statuses.FirstOrDefault(x => x.Id == AssignmentContractDisplay.Status.Id)!;
                AssignmentContractDisplay.EmployeeType = EmployeeTypes.FirstOrDefault(x => x.Id == AssignmentContractDisplay.EmployeeType.Id)!;
            }

            IsInitialize = true;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            if (value is ValueTuple<EmployeeDetailsDisplay, AssignmentContractDisplay> tuple)
            {
                CurrentEmployeeDetails = tuple.Item1;
                AssignmentContractDisplay = tuple.Item2;
            }
        }

        #region Display

        private EmployeeDetailsDisplay _currentEmployeeDetails = null!;
        public EmployeeDetailsDisplay CurrentEmployeeDetails
        {
            get => _currentEmployeeDetails;
            set => SetProperty(ref _currentEmployeeDetails, value);
        }

        public ObservableCollection<DepartmentDisplay> Departments { get; private init; } = [];
        private async Task GetAllDepartmentAsync()
        {
            var departments = await _departmentReadOnlyService.GetAllAsync();

            Departments.Load([.. departments.Select(department => new DepartmentDisplay(department))]);
        }

        public ObservableCollection<PositionDisplay> Positions { get; private init; } = [];
        private async Task GetAllPositionByIdDepartmentAsync()
        {
            var positions = await _positionReadOnlyApiServiceFactory.Create(AssignmentContractDisplay.Department.Id).GetAllAsync();

            Positions.Load([.. positions.Select(position => new PositionDisplay(position))], cleaning: true);
        }

        public ObservableCollection<StatusDisplay> Statuses { get; private init; } = [];
        private async Task GetAllStatusAsync()
        {
            var statuses = await _statusReadOnlyService.GetAllAsync();

            Statuses.Load([.. statuses.Select(status => new StatusDisplay(status))]);
        }

        public ObservableCollection<EmployeeTypeDisplay> EmployeeTypes { get; private init; } = [];
        private async Task GetAllEmployeeTypeAsync()
        {
            var employeeTypes = await _employeeTypeReadOnlyService.GetAllAsync();

            EmployeeTypes.Load([.. employeeTypes.Select(employeeType => new EmployeeTypeDisplay(employeeType))]);
        }

        public ObservableCollection<ManagerDisplay> Managers { get; private init; } = [];
        private async Task GetAllManagerAsync()
        {
            var managers = await _employeeService.GetAllAsync();

            Managers.Load([.. managers.Select(manager => new ManagerDisplay(manager))]);
        }

        private AssignmentContractDisplay _assignmentContractDisplay = null!;
        public AssignmentContractDisplay AssignmentContractDisplay
        {
            get => _assignmentContractDisplay;
            set
            {
                SetProperty(ref _assignmentContractDisplay, value);

                AssignmentContractDisplay.PropertyChanged += async (s, e) =>
                {
                    if (e.PropertyName == nameof(AssignmentContractDisplay.Department))
                        await GetAllPositionByIdDepartmentAsync();
                };
            }
        }

        #endregion

        public RelayCommandAsync? UpdateAssignmentEmployeeCommand { get; private set; }
        private async Task Execute_UpdateAssignmentEmployeeCommand()
        {
            try
            {
                if (AssignmentContractDisplay == null)
                {
                    MessageBox.Show("Данные о назначении не загружены");
                    return;
                }

                var resultUpdate = await _assignmentApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdateAssignmentAsync
                    (
                        Guid.Parse(AssignmentContractDisplay.AssignmentId),
                        Guid.Parse(AssignmentContractDisplay.ContractId),
                        new UpdateAssignmentRequest
                            (
                                Guid.Parse(AssignmentContractDisplay.Position.Id),
                                Guid.Parse(AssignmentContractDisplay.Department.Id),
                                AssignmentContractDisplay.Manager?.Id != null ? Guid.Parse(AssignmentContractDisplay.Manager.Id) : null,
                                AssignmentContractDisplay.HireDate,
                                AssignmentContractDisplay.TerminationDate,
                                Guid.Parse(AssignmentContractDisplay.Status.Id),
                                new UpdateAssignmentContractRequest
                                    (
                                        Guid.Parse(AssignmentContractDisplay.EmployeeType.Id),
                                        AssignmentContractDisplay.ContractNumber,
                                        AssignmentContractDisplay.StartDate,
                                        AssignmentContractDisplay.EndDate,
                                        AssignmentContractDisplay.Salary,
                                        null
                                    )
                            )
                    );

                if (resultUpdate.IsSuccess)
                    _navigationPage.TransmittingValue(AssignmentContractDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Update);
                else
                    MessageBox.Show($"Обновление назначения: {resultUpdate.StringMessage}");
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show($"Ошибка данных: {ex.ParamName} - {ex.Message}");
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка преобразования GUID: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}");
            }

            //var resultUpdate = await _assignmentApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdateAssignmentAsync
            //    (
            //        Guid.Parse(AssignmentContractDisplay.AssignmentId),
            //        Guid.Parse(AssignmentContractDisplay.ContractId),
            //        new UpdateAssignmentRequest
            //            (
            //                Guid.Parse(AssignmentContractDisplay.Position.Id),
            //                Guid.Parse(AssignmentContractDisplay.Department.Id),
            //                AssignmentContractDisplay.Manager?.Id != null ? Guid.Parse(AssignmentContractDisplay.Manager.Id) : null,
            //                AssignmentContractDisplay.HireDate,
            //                AssignmentContractDisplay.TerminationDate,
            //                Guid.Parse(AssignmentContractDisplay.Status.Id),
            //                new UpdateAssignmentContractRequest
            //                    (
            //                        Guid.Parse(AssignmentContractDisplay.EmployeeType.Id),
            //                        AssignmentContractDisplay.ContractNumber,
            //                        AssignmentContractDisplay.StartDate,
            //                        AssignmentContractDisplay.EndDate,
            //                        AssignmentContractDisplay.Salary,
            //                        null
            //                    )
            //            )
            //    );

            //if (resultUpdate.IsSuccess)
            //    _navigationPage.TransmittingValue(AssignmentContractDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Update);
            //else
            //    MessageBox.Show($"Обновление назначения: {resultUpdate.StringMessage}");
        }
        private bool CanExecute_UpdateAssignmentEmployeeCommand()
            => AssignmentContractDisplay != null &&
               AssignmentContractDisplay.Position != null &&
               AssignmentContractDisplay.Department != null &&
               AssignmentContractDisplay.Status != null &&
               AssignmentContractDisplay.EmployeeType != null &&
               AssignmentContractDisplay.HireDate != DateTime.MinValue &&
               !string.IsNullOrWhiteSpace(AssignmentContractDisplay.ContractNumber) &&
               AssignmentContractDisplay.StartDate != DateTime.MinValue &&
               AssignmentContractDisplay.EndDate != DateTime.MinValue &&
               AssignmentContractDisplay.Salary > 0;
    }
}
