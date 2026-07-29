using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Services.FilePreview;
using LifeLine.HrPanel.Desktop.Services.GenerateImage;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee;
using LifeLine.HrPanel.Desktop.Views.UserControls;
using Microsoft.Windows.Input;
using Shared.Contracts.Request.EmployeeService.Employee;
using Shared.Contracts.Request.Files;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.Conversion;
using Shared.WPF.Services.FileDialog;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class PersonalPhotoVM : BaseEmployeeViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePreviewService _filePreviewService;
        private readonly IGenerateImageService _generateImageSourse;
        private readonly IImageCompressionService _imageCompressionService;
        private readonly ManagementEmployeeStateService _stateService;

        public PersonalPhotoVM
            (
                IEmployeeService employeeService,
                IFileDialogService fileDialogService,
                IFileStorageService fileStorageService,
                IFilePreviewService filePreviewService, 
                IGenerateImageService generateImageService,
                IImageCompressionService imageCompressionService,
                ManagementEmployeeStateService stateService
            )
        {
            _employeeService = employeeService;
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;
            _filePreviewService = filePreviewService;
            _generateImageSourse = generateImageService;
            _imageCompressionService = imageCompressionService;
            _stateService = stateService;

            SelectCommandAsync = new RelayCommandAsync(Execute_SelectCommandAsync);
            PreviewCommandAsync = new RelayCommandAsync(Execute_PreviewCommandAsync);
            UploadPersonalPhotoAsync = new RelayCommandAsync(Execute_UploadPersonalPhotoAsync);
            DeleteImageCommandAsync = new RelayCommandAsync(Execute_DeleteImageCommandAsync);
        }

        public string? PhotoUrl
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public ImageSource? Photo
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        private byte[]? _compressedBytes;
        private string? _fileName;

        public RelayCommandAsync? SelectCommandAsync { get; private set; }
        private async Task Execute_SelectCommandAsync()
        {
            var path = _fileDialogService.GetFile($"Выберите файл: {FileDialogConsts.AVATAR}", FileFilters.Images);

            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
                return;

            try
            {
                var originalBytes = await System.IO.File.ReadAllBytesAsync(path);
                _compressedBytes = await _imageCompressionService.CompressImageAsync
                    (
                        originalBytes,
                        fileName: path,
                        quality: 85,
                        maxDimension: 512,
                        cancellationToken: default
                    );

                _fileName = Path.GetFileName(path);
                Photo = FileHelper.ImageFromFilePath(path);
                PhotoUrl = path;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommandAsync? PreviewCommandAsync { get; private set; }
        private async Task Execute_PreviewCommandAsync()
        {
            if (string.IsNullOrWhiteSpace(PhotoUrl) && Photo == null)
            {
                MessageBox.Show("Фотография отсутствует", "Предпросмотр",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                string? tempPath = null;
                bool isSaved = !string.IsNullOrWhiteSpace(PhotoUrl)
                               && PhotoUrl.Contains(":")
                               && !Path.IsPathFullyQualified(PhotoUrl);

                if (isSaved)
                {
                    var fileName = Path.GetFileName(PhotoUrl);
                    tempPath = await _filePreviewService.DownloadRemoteFileToTempAsync(PhotoUrl, fileName);
                }
                else if (!string.IsNullOrWhiteSpace(PhotoUrl) && System.IO.File.Exists(PhotoUrl))
                {
                    var fileName = _fileName ?? Path.GetFileName(PhotoUrl);
                    tempPath = _filePreviewService.CopyLocalFileToTempAsync(PhotoUrl, fileName);
                }

                if (string.IsNullOrWhiteSpace(tempPath))
                {
                    MessageBox.Show("Не удалось подготовить файл для просмотра", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _filePreviewService.OpenInDefaultApplication(tempPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PersonalPhotoVM] [PreviewCommand] Ошибка: {ex.Message}");
                MessageBox.Show($"Ошибка при открытии фотографии: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Func<Task>? OnPhotoUpdated;

        public RelayCommandAsync UploadPersonalPhotoAsync { get; private set; }
        private async Task Execute_UploadPersonalPhotoAsync()
        {
            var avatarBytes = GetCompressedBytes();
            var fileName = GetFileName();

            if (_stateService.EmployeeHr == null)
            {
                MessageBox.Show("Сотрудник не выбран!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (avatarBytes == null || string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Аватарка не нвыбрана!");
                return;
            }

            var fileResult = await _fileStorageService.UploadFileAsync
            (
                new UploadFileRequest
                (
                    FileConst.BUCKET_NAME,
                    nameof(PersonalPhotoVM),
                    FileConst.BuildEmployeeFolder
                    (
                        _stateService.EmployeeHr.Id,
                        EmployeeFolderType.PersonalPhoto
                    ),
                    FileBytes: avatarBytes,
                    FileName: fileName
                )
            );

            if (fileResult.IsFailure)
            {
                MessageBox.Show(fileResult.StringMessage);
                return;
            }

            var dbResult = await _employeeService.AddPersonalPhoto
            (
                _stateService.EmployeeHr.Id,
                new AddPersonalPhotoRequest
                (
                    FileConst.BUCKET_NAME,
                    fileResult.Value!.FileName
                )
            );

            if (dbResult.IsFailure)
            {
                MessageBox.Show(dbResult.StringMessage);
                return;
            }

            PhotoUrl = $"{FileConst.BUCKET_NAME}:{fileResult.Value.FileName}";

            var employeeHrResponse = _stateService.EmployeeHr with { PersonalPhoto = $"{FileConst.BUCKET_NAME}:{fileResult.Value.FileName}" };

            _stateService.UpdateEmployeeData(employeeHrResponse);

            if (OnPhotoUpdated != null)
                await OnPhotoUpdated.Invoke();
        }

        public RelayCommandAsync DeleteImageCommandAsync { get; private set; }
        private async Task Execute_DeleteImageCommandAsync()
        {
            if (string.IsNullOrWhiteSpace(PhotoUrl) && _compressedBytes == null && Photo == null)
            {
                ClearProperty();
                return;
            }

            bool isSaved = !string.IsNullOrWhiteSpace(PhotoUrl)
                           && PhotoUrl.Contains(":")
                           && !Path.IsPathFullyQualified(PhotoUrl);

            if (!isSaved)
            {
                ClearProperty();
                return;
            }

            try
            {
                if (_stateService.EmployeeHr == null)
                    return;

                var result = await _employeeService.DeletePersonalPhoto(_stateService.EmployeeHr.Id);

                if (result.IsFailure)
                {
                    MessageBox.Show(result.StringMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(PhotoUrl))
                    return;

                var (BucketName, FileName) = S3UrlParser.Parse(PhotoUrl);

                var fileResult = await _fileStorageService.DeleteFileAsync(new DeleteFileRequest(BucketName!, FileName!));

                if (fileResult.IsFailure)
                {
                    MessageBox.Show($"Ошибка удаления файла из S3 хранилища!\n{fileResult.StringMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ClearProperty();

                var employeeHrResponse = _stateService.EmployeeHr with { PersonalPhoto = string.Empty };

                _stateService.UpdateEmployeeData(employeeHrResponse);

                if (OnPhotoUpdated != null)
                    await OnPhotoUpdated.Invoke();
                else
                    Debug.WriteLine("[DeleteImageCommand] ВНИМАНИЕ: OnPhotoUpdated = null!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DeleteImageCommand] Критическая ошибка: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка при удалении фотографии: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public byte[]? GetCompressedBytes() => _compressedBytes;
        public string? GetFileName() => _fileName;

        public void ClearProperty()
        {
            PhotoUrl = string.Empty;
            Photo = null;
            _compressedBytes = null;
            _fileName = string.Empty;
        }
    }
}
