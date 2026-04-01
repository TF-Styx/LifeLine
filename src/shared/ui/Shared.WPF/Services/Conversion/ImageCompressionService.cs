using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Shared.WPF.Services.Conversion
{
    public class ImageCompressionService : IImageCompressionService
    {
        public async Task<byte[]> CompressImageAsync
            (
                byte[] fileData,
                string? fileName = null,
                int quality = 80,
                int maxDimension = 2480,
                CancellationToken cancellationToken = default
            )
        {
            return await Task.Run(() =>
            {
                // 👇 1. Проверяем, не является ли файл уже PDF
                if (IsPdfFile(fileData, fileName))
                    return fileData; // PDF не сжимаем

                // 👇 2. Пытаемся обработать как изображение
                try
                {
                    using var ms = new MemoryStream(fileData);
                    using var originalImage = Image.FromStream(ms);

                    var resizedImage = ResizeImage(originalImage, maxDimension);

                    using var outputMs = new MemoryStream();

                    // Сохраняем в JPEG для уменьшения размера
                    var encoder = GetEncoder(ImageFormat.Jpeg);
                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    resizedImage.Save(outputMs, encoder, encoderParams);
                    return outputMs.ToArray();
                }
                catch (Exception ex) when (ex is ArgumentException || ex is ExternalException)
                {
                    // 👇 3. Если не удалось распознать как изображение — возвращаем оригинал
                    // (возможно, это вебп, тифф или другой неподдерживаемый формат)
                    return fileData;
                }
            }, cancellationToken);
        }

        private static bool IsPdfFile(byte[] fileData, string? fileName)
        {
            // Проверка по расширению
            if (!string.IsNullOrWhiteSpace(fileName) &&
                Path.GetExtension(fileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                return true;

            // Проверка по магическим байтам (PDF начинается с "%PDF-")
            if (fileData.Length >= 5)
            {
                var signature = System.Text.Encoding.ASCII.GetString(fileData, 0, 5);
                return signature == "%PDF-";
            }

            return false;
        }

        private static Image ResizeImage(Image image, int maxDimension)
        {
            if (image.Width <= maxDimension && image.Height <= maxDimension)
                return image;

            float ratio = Math.Min
                (
                    (float)maxDimension / image.Width,
                    (float)maxDimension / image.Height
                );

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            var resized = new Bitmap(newWidth, newHeight);

            using (var gfx = Graphics.FromImage(resized))
            {
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return resized;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
            => ImageCodecInfo.GetImageEncoders()
                    .FirstOrDefault(codec => codec.FormatID == format.Guid)
                    ?? throw new InvalidOperationException("Кодировщик не найден");
    }
}
