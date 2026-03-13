using LifeLine.AdminPanel.Desktop.Models;
using LifeLine.Directory.Service.Client.Services.Department;
using Shared.Contracts.Request.DirectoryService.Department;
using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;

namespace LifeLine.AdminPanel.Desktop.ViewModels.Pages
{
    internal sealed class EditDepartmentPageVM : BasePageViewModel, IAsyncInitializable
    {
        private readonly IDepartmentService _departmentService;

        public EditDepartmentPageVM(IDepartmentService departmentService)
        {
            _departmentService = departmentService;

            CreateNewDepartment();

            AddDepartmentCommandAsync = new RelayCommandAsync(Execute_AddDepartmentCommandAsync, CanExecute_AddDepartmentCommandAsync);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetDepartmentAsync();

            IsInitialize = true;
        }

        private DepartmentDisplay _newDepartment = null!;
        public DepartmentDisplay NewDepartment
        {
            get => _newDepartment;
            private set => SetProperty(ref _newDepartment, value);
        }
        private void CreateNewDepartment()
        {
            NewDepartment = new(new DepartmentResponse(Guid.Empty, string.Empty, string.Empty));

            NewDepartment.PropertyChanged += (s, e) => AddDepartmentCommandAsync?.RaiseCanExecuteChanged();
        }

        public ObservableCollection<DepartmentDisplay> Departments = [];
        private async Task GetDepartmentAsync()
        {
            var departmentResponse = await _departmentService.GetAllAsync();

            foreach (var item in departmentResponse)
                Departments.Add(new DepartmentDisplay(item));
        }

        public RelayCommandAsync AddDepartmentCommandAsync { get; private set; }
        private async Task Execute_AddDepartmentCommandAsync()
        {
            //var departmentResult = await _departmentService.AddAsync
            //    (
            //        new CreateDepartmentRequest
            //            (
            //                NewDepartment.Name,
            //                NewDepartment.Description,
            //                Positions.Select().ToList(),
            //            )
            //    );
        }
        private bool CanExecute_AddDepartmentCommandAsync() => true;
    }
}
