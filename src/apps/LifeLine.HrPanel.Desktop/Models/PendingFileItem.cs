using Shared.WPF.ViewModels.Abstract;
using System.IO;
using System.Windows.Media.Imaging;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class PendingFileItem : BaseViewModel
    {
        private int _index;
        private string _fileName = null!;
        private string _filePath = null!;
        private BitmapImage? _thumbnail;
        private bool _isSelected;

        public PendingFileItem(int index, string filePath)
        {
            Index = index;
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);

            // 👇 Генерируем превью при создании (лениво, в фоне)
            _ = GenerateThumbnailAsync();
        }

        /// <summary>
        /// Порядковый номер файла в очереди (1, 2, 3...)
        /// </summary>
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        /// <summary>
        /// Имя файла с расширением
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        /// <summary>
        /// Полный путь к файлу на диске
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        /// <summary>
        /// Превью изображения (для отображения в UI)
        /// </summary>
        public BitmapImage? Thumbnail
        {
            get => _thumbnail;
            set => SetProperty(ref _thumbnail, value);
        }

        /// <summary>
        /// Выбран ли элемент в списке (для визуального выделения)
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// Размер файла в удобном формате (например, "2.4 МБ")
        /// </summary>
        public string FileSizeFormatted
        {
            get
            {
                try
                {
                    var size = new FileInfo(FilePath).Length;
                    return FormatBytes(size);
                }
                catch
                {
                    return "—";
                }
            }
        }

        /// <summary>
        /// Расширение файла в верхнем регистре (PDF, JPG, PNG)
        /// </summary>
        public string FileExtension => Path.GetExtension(FilePath).TrimStart('.').ToUpper();

        #region Helpers

        private async Task GenerateThumbnailAsync()
        {
            try
            {
                // 👇 Для PDF можно показать иконку, для изображений — превью
                if (IsImageFile(FilePath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(FilePath);
                    bitmap.DecodePixelWidth = 150;  // 👇 Оптимизация памяти
                    bitmap.DecodePixelHeight = 150;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();  // 👇 Делаем потокобезопасным для WPF

                    Thumbnail = bitmap;
                }
                else if (IsPdfFile(FilePath))
                {
                    // 👇 Для PDF можно показать заглушку или первую страницу (сложнее)
                    // Пока — просто заглушка, можно доработать через PdfSharp
                    Thumbnail = CreatePdfPlaceholder();
                }
            }
            catch
            {
                // 👇 Если не удалось создать превью — оставляем null
                Thumbnail = null;
            }
        }

        private static bool IsImageFile(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            return ext is ".jpg" or ".jpeg" or ".png" or ".bmp" or ".gif";
        }

        private static bool IsPdfFile(string path)
        {
            return Path.GetExtension(path).ToLower() == ".pdf";
        }

        private static BitmapImage CreatePdfPlaceholder()
        {
            // 👇 Простая заглушка для PDF (можно заменить на иконку из ресурсов)
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            // Если у вас есть иконка в ресурсах:
            // bitmap.UriSource = new Uri("pack://application:,,,/LifeLine.HrPanel.Desktop;component/Resources/pdf-icon.png");
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        private static string FormatBytes(long bytes)
        {
            string[] suffixes = { "Б", "КБ", "МБ", "ГБ", "ТБ" };
            int counter = 0;
            decimal number = bytes;

            while (Math.Round(number / 1024) >= 1 && counter < suffixes.Length - 1)
            {
                number /= 1024;
                counter++;
            }

            return $"{number:0.##} {suffixes[counter]}";
        }

        #endregion
    }
}
