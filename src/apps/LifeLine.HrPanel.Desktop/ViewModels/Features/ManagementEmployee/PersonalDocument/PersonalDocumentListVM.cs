using LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument
{
    public class PersonalDocumentListVM : BaseDocumentListVM<PersonalDocumentDisplay>
    {
        private readonly IPersonalDocumentApiServiceFactory _personalDocumentApiServiceFactory;

        public PersonalDocumentListVM
            (
                ManagementEmployeeStateServcie stateServcie, 
                IPersonalDocumentApiServiceFactory personalDocumentApiServiceFactory
            ) : base (stateServcie)
        {
            _personalDocumentApiServiceFactory = personalDocumentApiServiceFactory;

            EditCommand = new RelayCommand<PersonalDocumentDisplay?>(Execute_EditCommand);
        }

        public ObservableCollection<PersonalDocumentDisplay> PersonalDocuments { get; private init; } = [];
        protected override async Task LoadAsync(string employeeId)
        {
            var personalDocuments = await _personalDocumentApiServiceFactory.Create(employeeId)
                .GetAllByEmployeeId(employeeId);

            if (personalDocuments.IsFailure)
            {
                MessageBox.Show(personalDocuments.StringMessage);
                return;
            }

            PersonalDocuments.Load([.. personalDocuments.Value.Select(personalDocument => new PersonalDocumentDisplay(personalDocument, [], SaveStatus.DataBase))], cleaning: true);
        }

        private PersonalDocumentDisplay? _personalDocument;
        public PersonalDocumentDisplay? PersonalDocument
        {
            get => _personalDocument;
            set => SetProperty(ref _personalDocument, value);
        }

        protected override async Task DeleteAsync(string employeeId, PersonalDocumentDisplay display)
        {
            var result = await _personalDocumentApiServiceFactory.Create(employeeId)
                .DeletePersonalDocumentAsync(display.PersonalDocumentId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                throw new Exception(result.StringMessage);
            }
        }

        public Func<PersonalDocumentDisplay?, Task>? RequestEdit;
        public RelayCommand<PersonalDocumentDisplay?> EditCommand { get; private set; }
        private void Execute_EditCommand(PersonalDocumentDisplay? display) => RequestEdit?.Invoke(display);

    }
}
