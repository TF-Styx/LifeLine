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
        public async Task<byte[]> ConvertImagesToPdfAsync(List<byte[]> images, string documentType, string employeeId, CancellationToken cancellationToken = default)
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

                foreach (var imageData in images)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        // Проверяем, является ли файл уже PDF
                        if (IsPdfFile(imageData))
                        {
                            // Если это PDF, добавляем его страницы в наш документ
                            using var ms = new MemoryStream(imageData);
                            using var sourcePdf = PdfReader.Open(ms, PdfDocumentOpenMode.Import);

                            for (int i = 0; i < sourcePdf.PageCount; i++)
                            {
                                var page = document.AddPage(sourcePdf.Pages[i]);
                            }
                        }
                        else
                        {
                            // Обрабатываем как изображение
                            byte[] processedBytes;
                            try
                            {
                                // Пытаемся сжать изображение
                                processedBytes = imageCompressionService
                                    .CompressImageAsync(imageData, cancellationToken: cancellationToken)
                                    .GetAwaiter().GetResult();
                            }
                            catch
                            {
                                // Если сжатие не удалось, используем оригинал
                                processedBytes = imageData;
                            }

                            var page = document.AddPage();
                            page.Size = PageSize.A4;

                            using var gfx = XGraphics.FromPdfPage(page);

                            // Пытаемся загрузить изображение
                            using var ms = new MemoryStream(processedBytes);
                            XImage img;

                            try
                            {
                                img = XImage.FromStream(() => ms);
                            }
                            catch (Exception ex)
                            {
                                // Если не удалось загрузить как изображение, пропускаем с ошибкой
                                throw new InvalidOperationException(
                                    $"Не удалось загрузить изображение. Поддерживаемые форматы: JPEG, PNG, BMP, GIF, TGA. " +
                                    $"Ошибка: {ex.Message}");
                            }

                            using (img)
                            {
                                // Защита от деления на ноль
                                if (img.PixelWidth == 0 || img.PixelHeight == 0)
                                    continue;

                                // Расчет пропорций для вписывания в А4 с отступами
                                double pageWidth = page.Width.Point - 40;
                                double pageHeight = page.Height.Point - 40;
                                double imgRatio = (double)img.PixelWidth / img.PixelHeight;
                                double pageRatio = pageWidth / pageHeight;

                                double drawWidth, drawHeight;
                                if (imgRatio > pageRatio)
                                {
                                    drawWidth = pageWidth;
                                    drawHeight = pageWidth / imgRatio;
                                }
                                else
                                {
                                    drawHeight = pageHeight;
                                    drawWidth = pageHeight * imgRatio;
                                }

                                double x = (page.Width.Point - drawWidth) / 2;
                                double y = (page.Height.Point - drawHeight) / 2;

                                gfx.DrawImage(img, x, y, drawWidth, drawHeight);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Логируем ошибку для конкретного файла, но продолжаем обработку остальных
                        System.Diagnostics.Debug.WriteLine($"Ошибка при обработке файла: {ex.Message}");
                        // Можно добавить коллекцию ошибок и вернуть их вместе с результатом
                        throw; // Или продолжить обработку следующего файла
                    }
                }

                using var outputMs = new MemoryStream();
                document.Save(outputMs, false);
                return outputMs.ToArray();
            }, cancellationToken);
        }

        private static bool IsPdfFile(byte[] fileData)
        {
            // PDF файлы начинаются с "%PDF-"
            if (fileData.Length >= 5)
            {
                var signature = System.Text.Encoding.ASCII.GetString(fileData, 0, 5);
                return signature == "%PDF-";
            }
            return false;
        }
    }
}
