using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class PersonalDocumentsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        private readonly IReadOnlyCollection<DocumentTypeDisplay> _documentTypes;

        public PersonalDocumentsVM(IFileDialogService fileDialogService, IReadOnlyCollection<DocumentTypeDisplay> documentTypes)
        {
            _fileDialogService = fileDialogService;

            _documentTypes = documentTypes;

            //CreateNewPersonalDocument();

            SelectCommand = new RelayCommand(Execute_SelectCommand);
            AddPersonalDocumentCommand = new RelayCommand(Execute_AddPersonalDocumentCommand, CanExecute_AddPersonalDocumentCommand);
            DeletePersonalDocumentCommand = new RelayCommand<PersonalDocumentDisplay>(Execute_DeletePersonalDocumentCommand);
        }

        private string _number = null!;
        public string Number
        {
            get => _number;
            set
            {
                SetProperty(ref _number, value);
                AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private string? _series;
        public string? Series
        {
            get => _series;
            set
            {
                SetProperty(ref _series, value);
                AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private DocumentTypeDisplay _documentType = null!;
        public DocumentTypeDisplay DocumentType
        {
            get => _documentType;
            set
            {
                SetProperty(ref _documentType, value);

                AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public RelayCommand SelectCommand { get; private set; }
        private void Execute_SelectCommand()
            => FilePath = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.PERSONAL_DOCUMENT}", FileFilters.Images);

        private PersonalDocumentDisplay _newPersonalDocument;
        private void CreateNewPersonalDocument() 
            => _newPersonalDocument = new(new PersonalDocumentResponse(Guid.Empty, Guid.Empty, string.Empty, string.Empty), [], string.Empty);

        public void ClearProperty()
        {
            Number = string.Empty;
            Series = string.Empty;
            DocumentType = null!;
            FilePath = string.Empty;
        }

        private PersonalDocumentDisplay _selectedLocalPersonalDocument = null!;
        public PersonalDocumentDisplay SelectedLocalPersonalDocument
        {
            get => _selectedLocalPersonalDocument;
            set => SetProperty(ref _selectedLocalPersonalDocument, value);
        }

        public ObservableCollection<PersonalDocumentDisplay> LocalPersonalDocuments { get; private init; } = [];

        public RelayCommand? AddPersonalDocumentCommand { get; private set; }
        private void Execute_AddPersonalDocumentCommand()
        {
            //_newPersonalDocument.DocumentNumber = Number;
            //_newPersonalDocument.DocumentSeries = Series;
            //_newPersonalDocument.DocumentType = DocumentType;
            //_newPersonalDocument.FilePath = FilePath;

            LocalPersonalDocuments.Add
                (
                    new PersonalDocumentDisplay
                        (
                            new PersonalDocumentResponse
                                (
                                    Guid.Empty,
                                    Guid.Parse(DocumentType.Id),
                                    Number,
                                    Series
                                ),
                            _documentTypes,
                            FilePath
                        )
                );

            //CreateNewPersonalDocument();

            ClearProperty();
        }
        private bool CanExecute_AddPersonalDocumentCommand()
            => DocumentType != null && !string.IsNullOrWhiteSpace(Number);

        public RelayCommand<PersonalDocumentDisplay>? DeletePersonalDocumentCommand { get; private set; }
        private void Execute_DeletePersonalDocumentCommand(PersonalDocumentDisplay display)
            => LocalPersonalDocuments.Remove(display);
    }
}
