using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.DirectoryService.Department;
using Shared.Contracts.Request.DirectoryService.Position;
using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    internal sealed class EditDepartmentPositionPageVM : BasePageViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly IDepartmentService _departmentService;
        private readonly IPositionApiServiceFactory _positionApiServiceFactory;

        public EditDepartmentPositionPageVM
            (
                IDepartmentService departmentService,
                IPositionApiServiceFactory positionApiServiceFactory
            )
        {
            _departmentService = departmentService;
            _positionApiServiceFactory = positionApiServiceFactory;

            CreateDepartmentCommandAsync = new RelayCommandAsync(Execute_CreateDepartmentCommandAsync, CanExecute_CreateDepartmentCommandAsync);
            UpdateDepartmentCommandAsync = new RelayCommandAsync(Execute_UpdateDepartmentCommandAsync, CanExecute_UpdateDepartmentCommandAsync);
            DeleteDepartmentCommandAsync = new RelayCommandAsync<DepartmentDisplay>(Execute_DeleteDepartmentCommandAsync);

            _getAllPositionByIdDepartmentCommandAsync = new RelayCommandAsync<DepartmentDisplay>(GetAllPositionByIdDepartmentCommandAsync);
            CreatePositionCommandAsync = new RelayCommandAsync(Execute_CreatePositionCommandAsync, CanExecute_CreatePositionCommandAsync);
            UpdatePositionCommandAsync = new RelayCommandAsync(Execute_UpdatePositionCommandAsync, CanExecute_UpdatePositionCommandAsync);
            DeletePositionCommandAsync = new RelayCommandAsync<PositionDisplay>(Execute_DeletePositionCommandAsync);
        }
        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetDepartmentsAsync();

            NewDepartmentDisplay();
            NewPostionDisplay();

            IsInitialize = true;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            throw new NotImplementedException();
        }

        #region Department

        // Property Department
        private DepartmentDisplay? _departmentDis;
        public DepartmentDisplay? DepartmentDis
        {
            get => _departmentDis;
            set
            {
                SetProperty(ref _departmentDis, value);

                CreateDepartmentCommandAsync?.RaiseCanExecuteChanged();
                UpdateDepartmentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewDepartmentDisplay()
        {
            DepartmentDis = new DepartmentDisplay
            (
                new DepartmentResponse
                (
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    new DepartmentDataAddressResponse
                    (
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty
                    )
                )
            );

            DepartmentDis.PropertyChanged += (s, e) =>
            {
                CreateDepartmentCommandAsync?.RaiseCanExecuteChanged();
                UpdateDepartmentCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        // List DepartmentDisplay
        public ObservableCollection<DepartmentDisplay> Departments { get; private init; } = [];

        // Get DepartmentDisplay
        private async Task GetDepartmentsAsync()
        {
            var departments = await _departmentService.GetAllAsync();

            Departments.Load([.. departments.Select(department => new DepartmentDisplay(department))]);
        }

        // Selected DepartmentDisplay
        private DepartmentDisplay? _department;
        public DepartmentDisplay? Department
        {
            get => _department;
            set
            {
                if (SetProperty(ref _department, value))
                {
                    if (value != null)
                    {
                        Positions.Clear();
                        _getAllPositionByIdDepartmentCommandAsync.Execute(value);
                    }
                    else
                        Positions.Clear();
                }

                CreateDepartmentCommandAsync?.RaiseCanExecuteChanged();
                UpdateDepartmentCommandAsync?.RaiseCanExecuteChanged();

                SetPropDepartment(value);
            }
        }

        // Установка значений
        private void SetPropDepartment(DepartmentDisplay? value)
        {
            if (value == null)
            {
                NewDepartmentDisplay();
                return;
            }

            DepartmentDis!.Name = value.Name;
            DepartmentDis!.Description = value.Description;
            DepartmentDis!.PostalCode = value.PostalCode;
            DepartmentDis!.Region = value.Region;
            DepartmentDis!.City = value.City;
            DepartmentDis!.Street = value.Street;
            DepartmentDis!.Building = value.Building;
        }

        // CREATE
        public RelayCommandAsync CreateDepartmentCommandAsync { get; private set; }
        private async Task Execute_CreateDepartmentCommandAsync()
        {
            if (DepartmentDis == null)
            {
                MessageBox.Show("Данные пусты!");
                return;
            }

            var request = new CreateDepartmentRequest
                (
                    DepartmentDis.Name,
                    DepartmentDis.Description,
                    new CreateDepartmentAddressRequestData
                        (
                            DepartmentDis.PostalCode,
                            DepartmentDis.Region,
                            DepartmentDis.City,
                            DepartmentDis.Street,
                            DepartmentDis.Building
                        )
                );

            var result = await _departmentService.AddAsync<CreateDepartmentRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Departments.Add
            (
                new DepartmentDisplay
                (
                    new DepartmentResponse
                    (
                        result.Value,
                        DepartmentDis.Name,
                        DepartmentDis.Description,
                        new DepartmentDataAddressResponse
                        (
                            DepartmentDis.PostalCode,
                            DepartmentDis.Region,
                            DepartmentDis.City,
                            DepartmentDis.Street,
                            DepartmentDis.Building
                        )
                    )
                )
            );

            NewDepartmentDisplay();
        }
        private bool CanExecute_CreateDepartmentCommandAsync()
            => !string.IsNullOrWhiteSpace(DepartmentDis?.Name) &&
               !string.IsNullOrWhiteSpace(DepartmentDis?.Description) &&
               !string.IsNullOrWhiteSpace(DepartmentDis?.PostalCode) &&
               !string.IsNullOrWhiteSpace(DepartmentDis?.Region) &&
               !string.IsNullOrWhiteSpace(DepartmentDis?.City) &&
               !string.IsNullOrWhiteSpace(DepartmentDis?.Street) &&
               !string.IsNullOrWhiteSpace(DepartmentDis?.Building);

        // UPDATE
        public RelayCommandAsync UpdateDepartmentCommandAsync { get; private set; }
        private async Task Execute_UpdateDepartmentCommandAsync()
        {
            if (Department == null)
            {
                MessageBox.Show("Не был выбран отдел!");
                return;
            }

            var request = new UpdateDepartmentRequest
            (
                DepartmentDis!.Name, 
                DepartmentDis!.Description,
                new UpdateDepartmentDataAddressRequest
                (
                    DepartmentDis!.PostalCode,
                    DepartmentDis!.Region,
                    DepartmentDis!.City,
                    DepartmentDis!.Street,
                    DepartmentDis!.Building
                )
            );

            var result = await _departmentService.UpdateAsync(Department.Id, request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var updateDepartment = Departments.FirstOrDefault(x => x.Id == Department.Id);

            if (updateDepartment == null)
            {
                MessageBox.Show("Не удалось обновить данные!");
                return;
            }

            updateDepartment.Name = DepartmentDis!.Name;
            updateDepartment.Description = DepartmentDis!.Description;

            updateDepartment.PostalCode = DepartmentDis!.PostalCode;
            updateDepartment.Region = DepartmentDis!.Region;
            updateDepartment.City = DepartmentDis!.City;
            updateDepartment.Street = DepartmentDis!.Street;
            updateDepartment.Building = DepartmentDis!.Building;

            NewDepartmentDisplay();
        }
        private bool CanExecute_UpdateDepartmentCommandAsync()
            => Department != null && 
               !string.IsNullOrWhiteSpace(DepartmentDis!.Name) &&
               !string.IsNullOrWhiteSpace(DepartmentDis!.Description) &&
               !string.IsNullOrWhiteSpace(DepartmentDis!.PostalCode) &&
               !string.IsNullOrWhiteSpace(DepartmentDis!.Region) &&
               !string.IsNullOrWhiteSpace(DepartmentDis!.City) &&
               !string.IsNullOrWhiteSpace(DepartmentDis!.Street) &&
               !string.IsNullOrWhiteSpace(DepartmentDis!.Building);

        // DELETE
        public RelayCommandAsync<DepartmentDisplay> DeleteDepartmentCommandAsync { get; private set; }
        private async Task Execute_DeleteDepartmentCommandAsync(DepartmentDisplay display)
        {
            var result = await _departmentService.DeleteAsync(display.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Departments.Remove(display);

            NewDepartmentDisplay();
        }

        #endregion

        #region Position

        // Property Position
        private PositionDisplay? _positionDis;
        public PositionDisplay? PositionDis
        {
            get => _positionDis;
            set
            {
                SetProperty(ref _positionDis, value);

                CreatePositionCommandAsync?.RaiseCanExecuteChanged();
                UpdatePositionCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewPostionDisplay()
        {
            PositionDis = new PositionDisplay(new PositionResponse(Guid.Empty, string.Empty, string.Empty));

            PositionDis.PropertyChanged += (s, e) =>
            {
                CreatePositionCommandAsync?.RaiseCanExecuteChanged();
                UpdatePositionCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        // List<PositionDisplay>
        public ObservableCollection<PositionDisplay> Positions { get; private init; } = [];

        // Get<PositionDisplay>
        private readonly RelayCommandAsync<DepartmentDisplay> _getAllPositionByIdDepartmentCommandAsync;
        private async Task GetAllPositionByIdDepartmentCommandAsync(DepartmentDisplay display)
        {
            if (display == null || display.Id == string.Empty)
            {
                Positions.Clear();
                return;
            }

            var positions = await _positionApiServiceFactory.Create(display.Id).GetAllAsync();

            Positions.Load([.. positions.Select(position => new PositionDisplay(position))]);
        }

        // Selected<PositionDisplay>
        private PositionDisplay? _position;
        public PositionDisplay? Position
        {
            get => _position;
            set
            {
                SetProperty(ref _position, value);

                CreatePositionCommandAsync?.RaiseCanExecuteChanged();
                UpdatePositionCommandAsync?.RaiseCanExecuteChanged();

                SetPropPosition(value);
            }
        }

        private void SetPropPosition(PositionDisplay? value)
        {
            if (value == null)
            {
                NewPostionDisplay();
                return;
            }

            PositionDis!.Name = value.Name;
            PositionDis!.Description = value.Description;
        }

        // CREATE
        public RelayCommandAsync CreatePositionCommandAsync { get; private set; }
        private async Task Execute_CreatePositionCommandAsync()
        {
            if (Department == null && PositionDis == null)
            {
                MessageBox.Show("Данные пусты!");
                return;
            }

            var request = new CreatePositionRequest(PositionDis!.Name, PositionDis!.Description);

            var result = await _positionApiServiceFactory.Create(Department!.Id).AddAsync<CreatePositionRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Positions.Add(new PositionDisplay(new PositionResponse(Guid.Parse(result.Value), PositionDis.Name, PositionDis.Description)));

            NewPostionDisplay();
        }
        private bool CanExecute_CreatePositionCommandAsync()
            => !string.IsNullOrWhiteSpace(PositionDis?.Name) &&
               !string.IsNullOrWhiteSpace(PositionDis?.Description);

        // UPDATE
        public RelayCommandAsync UpdatePositionCommandAsync { get; private set; }
        private async Task Execute_UpdatePositionCommandAsync()
        {
            if (Department == null && Position == null)
            {
                MessageBox.Show("Не был выбран или отдел или должность!");
                return;
            }

            var request = new UpdatePositionRequest(PositionDis!.Name, PositionDis!.Description);

            var result = await _positionApiServiceFactory.Create(Department!.Id).UpdateAsync(Position!.Id, request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var updatePosition = Positions.FirstOrDefault(x => x.Id == Position.Id);

            if (updatePosition == null)
            {
                MessageBox.Show("Не удалось обновить данные!");
                return;
            }

            updatePosition.Name = PositionDis!.Name;
            updatePosition.Description = PositionDis!.Description;

            NewPostionDisplay();
        }
        private bool CanExecute_UpdatePositionCommandAsync()
            => Department != null && Position != null &&
               !string.IsNullOrWhiteSpace(PositionDis?.Name) &&
               !string.IsNullOrWhiteSpace(PositionDis?.Description);

        // DELETE
        public RelayCommandAsync<PositionDisplay> DeletePositionCommandAsync { get; private set; }
        private async Task Execute_DeletePositionCommandAsync(PositionDisplay display)
        {
            var result = await _positionApiServiceFactory.Create(Department!.Id).DeleteAsync(display.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Positions.Remove(display);

            NewPostionDisplay();
        }

        #endregion
    }
}
