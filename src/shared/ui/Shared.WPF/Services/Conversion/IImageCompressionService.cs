namespace Shared.WPF.Services.Conversion;

public interface IImageCompressionService
{
    /// <summary>
    /// Сжимает изображение и возвращает поток с оптимизированными данными
    /// </summary>
    Task<byte[]> CompressImageAsync
        (
            byte[] fileData,
            string? fileName = null,
            int quality = 80,
            int maxDimension = 2480,
            CancellationToken cancellationToken = default
        );
}