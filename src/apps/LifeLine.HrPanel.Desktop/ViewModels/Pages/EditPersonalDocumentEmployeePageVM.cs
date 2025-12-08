using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.PersonalDocument;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class EditPersonalDocumentEmployeePageVM : BasePageViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly INavigationPage _navigationPage;

        private readonly IDocumentTypeReadOnlyService _documentTypeReadOnlyService;
        private readonly IPersonalDocumentApiServiceFactory _personalDocumentApiServiceFactory;

        public EditPersonalDocumentEmployeePageVM
            (
                INavigationPage navigationPage,

                IDocumentTypeReadOnlyService documentTypeReadOnlyService,
                IPersonalDocumentApiServiceFactory personalDocumentApiServiceFactory
            )
        {
            _navigationPage = navigationPage;

            _documentTypeReadOnlyService = documentTypeReadOnlyService;
            _personalDocumentApiServiceFactory = personalDocumentApiServiceFactory;

            UpdatePersonalDocumentEmployeeCommand = new RelayCommandAsync(Execute_UpdatePersonalDocumentEmployeeCommand, CanExecute_UpdatePersonalDocumentEmployeeCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllDocumentTypeAsync();

            if (PersonalDocumentDisplay != null)
                SelectedDocumentType = DocumentTypes.FirstOrDefault(x => x.Id == PersonalDocumentDisplay.DocumentType.Id)!;

            IsInitialize = true;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            if (value is ValueTuple<EmployeeDetailsDisplay, PersonalDocumentDisplay> tuple)
            {
                CurrentEmployeeDetails = tuple.Item1;
                PersonalDocumentDisplay = tuple.Item2;

                // Устанавливаем выбранный тип документа при приходе данных и обновляем CanExecute команды
                if (PersonalDocumentDisplay != null && DocumentTypes.Count > 0)
                {
                    SelectedDocumentType = DocumentTypes.FirstOrDefault(x => x.Id == PersonalDocumentDisplay.DocumentType.Id);
                }

                // Пересчитываем доступность команды (RelayCommandAsync наследует BaseCommand => RaiseCanExecuteChanged доступен)
                UpdatePersonalDocumentEmployeeCommand?.RaiseCanExecuteChanged();
            }
        }

        #region Display

        private EmployeeDetailsDisplay _currentEmployeeDetails = null!;
        public EmployeeDetailsDisplay CurrentEmployeeDetails
        {
            get => _currentEmployeeDetails;
            set => SetProperty(ref _currentEmployeeDetails, value);
        }

        private DocumentTypeDisplay? _selectedDocumentType;
        public DocumentTypeDisplay? SelectedDocumentType
        {
            get => _selectedDocumentType;
            set => SetProperty(ref _selectedDocumentType, value);
        }

        public ObservableCollection<DocumentTypeDisplay> DocumentTypes { get; private init; } = [];
        private async Task GetAllDocumentTypeAsync()
        {
            var documentTypes = await _documentTypeReadOnlyService.GetAllAsync();

            DocumentTypes.Load(documentTypes.Select(documentType => new DocumentTypeDisplay(documentType)).ToList());
        }

        private PersonalDocumentDisplay _personalDocumentDisplay = null!;
        public PersonalDocumentDisplay PersonalDocumentDisplay
        {
            get => _personalDocumentDisplay;
            set => SetProperty(ref _personalDocumentDisplay, value);
        }

        #endregion

        public RelayCommandAsync UpdatePersonalDocumentEmployeeCommand { get; private set; }
        private async Task Execute_UpdatePersonalDocumentEmployeeCommand()
        {
            var resultUpdate = await _personalDocumentApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdatePersonalDocumentAsync
                (
                    PersonalDocumentDisplay.PersonalDocumentId,
                    new UpdatePersonalDocumentRequest
                        (
                            SelectedDocumentType!.Id,
                            PersonalDocumentDisplay.DocumentNumber,
                            PersonalDocumentDisplay.DocumentSeries
                        )
                );

            if (resultUpdate.IsSuccess)
                _navigationPage.TransmittingValue(PersonalDocumentDisplay, FrameName.MainFrame, PageName.EmployeePage, TransmittingParameter.Update);
            else
                MessageBox.Show($"Обновление персональных документов: {resultUpdate.StringMessage}");
        }
        private bool CanExecute_UpdatePersonalDocumentEmployeeCommand() => SelectedDocumentType != null;
    }
}
