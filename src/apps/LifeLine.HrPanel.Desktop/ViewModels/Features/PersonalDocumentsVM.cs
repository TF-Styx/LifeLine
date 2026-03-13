using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class PersonalDocumentsVM : BaseViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        public PersonalDocumentsVM(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;

            SelectCommand = new RelayCommand(Execute_SelectCommand);
        }

        private string _number = null!;
        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        private string? _series;
        public string? Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        private DocumentTypeResponse _documentType = null!;
        public DocumentTypeResponse DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value);
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public RelayCommand SelectCommand { get; private set; }
        private void Execute_SelectCommand()
            => FilePath = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.PERSONAL_DOCUMENT}", FileFilters.Images);
    }
}
