using LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument;
using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument
{
    public class PersonalDocumentListVM
    {
        private readonly ManagementEmployeeStateServcie _stateServcie;
        private readonly IPersonalDocumentApiServiceFactory _personalDocumentApiServiceFactory;

        public PersonalDocumentListVM(ManagementEmployeeStateServcie stateServcie, IPersonalDocumentApiServiceFactory personalDocumentApiServiceFactory)
        {
            _stateServcie = stateServcie;
            _personalDocumentApiServiceFactory = personalDocumentApiServiceFactory;

            _stateServcie.EmployeeContextChanged += async employeeId =>
            {
                PersonalDocuments.Clear();

                if (!string.IsNullOrWhiteSpace(employeeId))
                    await GetPersonalDocumentsByEmployee(employeeId);
            };

            EditCommand = new RelayCommand<PersonalDocumentDisplay?>(Execute_EditCommand);
            DeleteCommandAsync = new RelayCommandAsync<PersonalDocumentDisplay>(Execute_DeleteCommandAsync);
        }

        public ObservableCollection<PersonalDocumentDisplay> PersonalDocuments { get; private init; } = [];
        private async Task GetPersonalDocumentsByEmployee(string employeeId)
        {
            var personalDocuments = await _personalDocumentApiServiceFactory.Create(employeeId).GetAllByEmployeeId(employeeId);

            if (personalDocuments.IsFailure)
            {
                MessageBox.Show(personalDocuments.StringMessage);
                return;
            }

            PersonalDocuments.Load([.. personalDocuments.Value.Select(personalDocument => new PersonalDocumentDisplay(personalDocument, [], SaveStatus.DataBase))], cleaning: true);
        }

        public Action<PersonalDocumentDisplay?>? RequestEdit;
        public RelayCommand<PersonalDocumentDisplay?> EditCommand { get; private set; }
        private void Execute_EditCommand(PersonalDocumentDisplay? display) => RequestEdit?.Invoke(display);

        public Action<PersonalDocumentDisplay?>? Deleted;
        public RelayCommandAsync<PersonalDocumentDisplay> DeleteCommandAsync { get; private set; }
        private async Task Execute_DeleteCommandAsync(PersonalDocumentDisplay display)
        {
            if (_stateServcie.EmployeeHr == null || string.IsNullOrWhiteSpace(_stateServcie.EmployeeHr.Id))
            {
                MessageBox.Show("Не был выбран сотрудник!");
                return;
            }

            var employeeId = _stateServcie.EmployeeHr.Id;

            var result = await _personalDocumentApiServiceFactory.Create(employeeId).DeletePersonalDocumentAsync(display.PersonalDocumentId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            PersonalDocuments.Remove(display);
            Deleted?.Invoke(display);
        }
    }
}
