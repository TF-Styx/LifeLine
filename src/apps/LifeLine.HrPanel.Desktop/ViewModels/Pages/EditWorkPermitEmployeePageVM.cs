using LifeLine.Directory.Service.Client.Services.AdmissionStatus;
using LifeLine.Directory.Service.Client.Services.PermitType;
using LifeLine.Employee.Service.Client.Services.Employee.WorkPermit;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.WorkPermit;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class EditWorkPermitEmployeePageVM : BaseViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly INavigationPage _navigationPage;

        private readonly IPermitTypeReadOnlyService _permitTypeReadOnlyService;
        private readonly IAdmissionStatusReadOnlyService _admissionStatusReadOnlyService;
        private readonly IWorkPermitApiServiceFactory _workPermitApiServiceFactory;

        public EditWorkPermitEmployeePageVM
            (
                INavigationPage navigationPage,

                IPermitTypeReadOnlyService permitTypeReadOnlyService,
                IAdmissionStatusReadOnlyService admissionStatusReadOnlyService,
                IWorkPermitApiServiceFactory workPermitApiServiceFactory
            )
        {
            _navigationPage = navigationPage;

            _permitTypeReadOnlyService = permitTypeReadOnlyService;
            _admissionStatusReadOnlyService = admissionStatusReadOnlyService;
            _workPermitApiServiceFactory = workPermitApiServiceFactory;

            UpdateWorkPermitEmployeeCommand = new RelayCommandAsync(Execute_UpdateWorkPermitEmployeeCommand, CanExecute_UpdateWorkPermitEmployeeCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllPermitTypeAsync();
            await GetAllAdmissionStatusAsync();

            if (WorkPermitDisplay != null && PermitTypes.Count > 0 && AdmissionStatuses.Count > 0)
            {
                WorkPermitDisplay.PermitType = PermitTypes.FirstOrDefault(x => x.Id == WorkPermitDisplay.PermitType.Id)!;
                WorkPermitDisplay.AdmissionStatus = AdmissionStatuses.FirstOrDefault(x => x.Id == WorkPermitDisplay.AdmissionStatus.Id)!;
            }

            IsInitialize = true;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            if (value is ValueTuple<EmployeeDetailsDisplay, WorkPermitDisplay> tuple)
            {
                CurrentEmployeeDetails = tuple.Item1;
                WorkPermitDisplay = tuple.Item2;

                if (WorkPermitDisplay != null && PermitTypes.Count > 0 && AdmissionStatuses.Count > 0)
                {
                    WorkPermitDisplay.PermitType = PermitTypes.FirstOrDefault(x => x.Id == WorkPermitDisplay.PermitType.Id)!;
                    WorkPermitDisplay.AdmissionStatus = AdmissionStatuses.FirstOrDefault(x => x.Id == WorkPermitDisplay.AdmissionStatus.Id)!;
                }

                UpdateWorkPermitEmployeeCommand?.RaiseCanExecuteChanged();
            }
        }

        #region Display

        private EmployeeDetailsDisplay _currentEmployeeDetails = null!;
        public EmployeeDetailsDisplay CurrentEmployeeDetails
        {
            get => _currentEmployeeDetails;
            set => SetProperty(ref _currentEmployeeDetails, value);
        }

        public ObservableCollection<PermitTypeDisplay> PermitTypes { get; private init; } = [];
        private async Task GetAllPermitTypeAsync()
        {
            var permiteTypes = await _permitTypeReadOnlyService.GetAllAsync();

            PermitTypes.Load([.. permiteTypes.Select(permitType => new PermitTypeDisplay(permitType))]);
        }

        public ObservableCollection<AdmissionStatusDisplay> AdmissionStatuses { get; private init; } = [];
        private async Task GetAllAdmissionStatusAsync()
        {
            var admissionStatuses = await _admissionStatusReadOnlyService.GetAllAsync();

            AdmissionStatuses.Load([.. admissionStatuses.Select(admissionStatus => new AdmissionStatusDisplay(admissionStatus))]);
        }

        private WorkPermitDisplay _workPermitDisplay = null!;
        public WorkPermitDisplay WorkPermitDisplay
        {
            get => _workPermitDisplay;
            set => SetProperty(ref _workPermitDisplay, value);
        }

        #endregion

        public RelayCommandAsync? UpdateWorkPermitEmployeeCommand { get; private set; }
        private async Task Execute_UpdateWorkPermitEmployeeCommand()
        {
            var resultUpdate = await _workPermitApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdateWorkPermitAsync
                (
                    Guid.Parse(WorkPermitDisplay.Id),
                    new UpdateWorkPermitRequest
                        (
                            WorkPermitDisplay.WorkPermitName,
                            WorkPermitDisplay.DocumentSeries,
                            WorkPermitDisplay.WorkPermitNumber,
                            WorkPermitDisplay.ProtocolNumber,
                            WorkPermitDisplay.SpecialtyName,
                            WorkPermitDisplay.IssuingAuthority,
                            WorkPermitDisplay.IssueDate,
                            WorkPermitDisplay.ExpiryDate,
                            WorkPermitDisplay.PermitType.Id,
                            WorkPermitDisplay.AdmissionStatus.Id
                        )
                );

            if (resultUpdate.IsSuccess)
                _navigationPage.TransmittingValue(WorkPermitDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Update);
            else
                MessageBox.Show($"Обновление разрешения на работу: {resultUpdate.StringMessage}");
        }
        private bool CanExecute_UpdateWorkPermitEmployeeCommand() => /*true;*/ WorkPermitDisplay.PermitType != null && WorkPermitDisplay.AdmissionStatus != null;
    }
}
