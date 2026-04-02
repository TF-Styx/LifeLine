using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.IO;
using XImage = PdfSharpCore.Drawing.XImage;

namespace Shared.WPF.Services.Conversion
{
    public class DocumentConversionService(IImageCompressionService imageCompressionService) : IDocumentConversionService
    {
        public async Task<byte[]> ConvertImagesToPdfAsync
            (
                string documentType, 
                string employeeId,
                List<byte[]> files,
                List<string>? fileNames = null,
                CancellationToken cancellationToken = default
            )
        {
            return await Task.Run(async () =>
            {
                using var document = new PdfDocument();

                document.Options.CompressContentStreams = true;
                document.Options.FlateEncodeMode = PdfFlateEncodeMode.Default;
                document.Options.NoCompression = false;

                // Метаданные
                document.Info.Title = $"Скан {documentType}";
                document.Info.Subject = $"Документ сотрудника #{employeeId}";
                document.Info.Keywords = documentType.ToLower();
                document.Info.Author = "LifeLine HR Panel";
                document.Info.Creator = "LifeLine HR Panel";
                document.Info.CreationDate = DateTime.UtcNow;

                for (int i = 0; i < files.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var fileData = files[i];
                    var fileName = fileNames?.ElementAtOrDefault(i);

                    // 👇 Определяем тип файла
                    if (IsPdfFile(fileData, fileName))
                    {
                        // 📄 Это PDF — извлекаем и добавляем все его страницы
                        MergePdfIntoDocument(document, fileData);
                    }
                    else
                    {
                        // 🖼️ Это изображение — сжимаем и добавляем как страницу
                        AddImageAsPage(document, fileData, fileName);
                    }
                }

                using var outputMs = new MemoryStream();
                document.Save(outputMs, false);
                return outputMs.ToArray();
            }, cancellationToken);
        }

        #region Helpers: PDF

        private static bool IsPdfFile(byte[] fileData, string? fileName)
        {
            // Проверка по расширению
            if (!string.IsNullOrWhiteSpace(fileName) &&
                Path.GetExtension(fileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Проверка по магическим байтам ("%PDF-")
            if (fileData.Length >= 5)
            {
                var signature = System.Text.Encoding.ASCII.GetString(fileData, 0, 5);
                if (signature == "%PDF-")
                {
                    return true;
                }
            }

            return false;
        }

        private static void MergePdfIntoDocument(PdfDocument targetDocument, byte[] sourcePdfBytes)
        {
            using var sourceMs = new MemoryStream(sourcePdfBytes);
            using var sourceDocument = PdfReader.Open(sourceMs, PdfDocumentOpenMode.Import);

            for (int i = 0; i < sourceDocument.PageCount; i++)
            {
                var page = sourceDocument.Pages[i];
                // 👇 ImportPage копирует страницу с полным содержимым
                targetDocument.AddPage(page);
            }
        }

        #endregion

        #region Helpers: Images

        private void AddImageAsPage(PdfDocument document, byte[] imageData, string? fileName)
        {
            try
            {
                // 👇 Сначала пробуем сжать изображение (если это не уже сжатый формат)
                var processedData = imageCompressionService.CompressImageAsync(
                    imageData,
                    fileName,
                    quality: 85,
                    maxDimension: 2480).GetAwaiter().GetResult();

                var page = document.AddPage();
                page.Size = PageSize.A4;
                page.Orientation = PageOrientation.Portrait;

                using var gfx = XGraphics.FromPdfPage(page);
                using var ms = new MemoryStream(processedData);
                var xImage = XImage.FromStream(() => ms);

                // 👇 Расчет пропорций для вписывания в А4 с отступами
                double pageWidth = page.Width.Point - 40;  // отступы по 20pt
                double pageHeight = page.Height.Point - 40;
                double imgRatio = (double)xImage.PixelWidth / xImage.PixelHeight;
                double pageRatio = pageWidth / pageHeight;

                double drawWidth, drawHeight;
                if (imgRatio > pageRatio)
                {
                    // Изображение шире страницы — масштабируем по ширине
                    drawWidth = pageWidth;
                    drawHeight = pageWidth / imgRatio;
                }
                else
                {
                    // Изображение выше страницы — масштабируем по высоте
                    drawHeight = pageHeight;
                    drawWidth = pageHeight * imgRatio;
                }

                // 👇 Центрируем на странице
                double x = (page.Width.Point - drawWidth) / 2;
                double y = (page.Height.Point - drawHeight) / 2;

                gfx.DrawImage(xImage, x, y, drawWidth, drawHeight);
            }
            catch (Exception)
            {
                // 👇 Если не удалось обработать как изображение — пропускаем с логированием
                // (возможно, повреждённый файл или неподдерживаемый формат)
                System.Diagnostics.Debug.WriteLine(
                    $"[DocumentConversion] Failed to process image: {fileName ?? "unknown"}");
            }
        }

        #endregion
    }
}
