using LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry;
using LifeLine.Employee.Service.Client.Services.Specialty;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class EditSpecialtyEmployeePageVM : BaseViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly INavigationPage _navigationPage;

        private readonly ISpecialtyReadOnlyService _specialtyReadOnlyService;
        private readonly IEmployeeSpecialtyApiServiceFactory _employeeSpecialtyApiServiceFactory;

        private bool _isEditMode;

        public EditSpecialtyEmployeePageVM
            (
                INavigationPage navigationPage, 
                ISpecialtyReadOnlyService specialtyReadOnlyService, 
                IEmployeeSpecialtyApiServiceFactory employeeSpecialtyApiServiceFactory
            )
        {
            _navigationPage = navigationPage;

            _specialtyReadOnlyService = specialtyReadOnlyService;
            _employeeSpecialtyApiServiceFactory = employeeSpecialtyApiServiceFactory;

            UpdateSpecialtyEmployeeCommand = new RelayCommandAsync(Execute_UpdateSpecialtyEmployeeCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllSpecialtyAsync();

            if (SpecialtyDisplay != null && Specialties.Count > 0)
                SelectedSpecialty = Specialties.FirstOrDefault(x => x.SpecialtyId == SpecialtyDisplay.SpecialtyId)!;

            IsInitialize = true;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            if (value is ValueTuple<EmployeeDetailsDisplay, SpecialtyDisplay> tuple)
            {
                CurrentEmployeeDetails = tuple.Item1;
                var incomingDocument = tuple.Item2;

                if (incomingDocument != null)
                {
                    _isEditMode = true;

                    SpecialtyDisplay = incomingDocument;

                    if (SpecialtyDisplay != null && Specialties.Count > 0)
                        SelectedSpecialty = Specialties.FirstOrDefault(x => x.SpecialtyId == SpecialtyDisplay.SpecialtyId)!;
                }
                else
                {
                    _isEditMode = false;

                    SpecialtyDisplay = new SpecialtyDisplay(new SpecialtyResponse(string.Empty, string.Empty, string.Empty));

                    SelectedSpecialty = null;
                }
            }

            UpdateSpecialtyEmployeeCommand?.RaiseCanExecuteChanged();
        }

        #region Display

        private EmployeeDetailsDisplay _currentEmployeeDetails = null!;
        public EmployeeDetailsDisplay CurrentEmployeeDetails
        {
            get => _currentEmployeeDetails;
            set => SetProperty(ref _currentEmployeeDetails, value);
        }

        private SpecialtyDisplay _specialtyDisplay = null!;
        public SpecialtyDisplay SpecialtyDisplay
        {
            get => _specialtyDisplay;
            set => SetProperty(ref _specialtyDisplay, value);
        }

        private SpecialtyDisplay _selectedSpecialty = null!;
        public SpecialtyDisplay SelectedSpecialty
        {
            get => _selectedSpecialty;
            set => SetProperty(ref _selectedSpecialty, value);
        }

        public ObservableCollection<SpecialtyDisplay> Specialties { get; private init; } = [];
        private async Task GetAllSpecialtyAsync()
        {
            var specialties = await _specialtyReadOnlyService.GetAllAsync();

            Specialties.Load(specialties.Select(specialty => new SpecialtyDisplay(specialty)).ToList());
        }

        #endregion

        public RelayCommandAsync? UpdateSpecialtyEmployeeCommand { get; private set; }
        private async Task Execute_UpdateSpecialtyEmployeeCommand()
        {
            if (_isEditMode)
            {
                var resultUpdate = await _employeeSpecialtyApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdateEmployeeSpecialtyAsync
                (
                    new UpdateEmployeeSpecialtyRequest
                        (
                            SpecialtyDisplay.SpecialtyId,
                            SelectedSpecialty.SpecialtyId
                        )
                );

                if (resultUpdate.IsSuccess)
                    _navigationPage.TransmittingValue((SpecialtyDisplay, SelectedSpecialty), FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Update);
                else
                    MessageBox.Show($"Обновление специальностей: {resultUpdate.StringMessage}");
            }
            else
            {
                var resultCreate = await _employeeSpecialtyApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).CreateAsync
                    (
                        new CreateEmployeeSpecialtyRequest(Guid.Parse(SelectedSpecialty.SpecialtyId))
                    );

                if (resultCreate.IsSuccess)
                    _navigationPage.TransmittingValue(SelectedSpecialty, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Create);
                else
                    MessageBox.Show($"Добавление специальностей: {resultCreate.StringMessage}");
            }
        }
    }
}
