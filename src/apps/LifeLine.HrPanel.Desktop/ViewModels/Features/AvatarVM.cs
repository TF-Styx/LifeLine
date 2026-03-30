using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Windows.Media;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class AvatarVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        public AvatarVM(IFileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;

            SelectCommand = new RelayCommand(Execute_SelectCommand);
        }

        public ImageSource? Ava
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        private string? _path;

        public RelayCommand? SelectCommand { get; private set; }
        private void Execute_SelectCommand()
        {
            _path = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.AVATAR}", FileFilters.Images);
            Ava = ImageHelper.ToImageFromFilePath(_path);
        }

        public string? GetPath() => _path;

        public void ClearProperty()
        {
            Ava = null;
            _path = string.Empty;
        }
    }
}
