namespace Shared.WPF.Services.Conversion;

public interface IDocumentConversionService
{
    /// <summary>
    /// Конвертирует список изображений в один PDF файл
    /// </summary>
    Task<byte[]> ConvertImagesToPdfAsync(List<byte[]> images, string documentType, string employeeId, CancellationToken cancellationToken = default);
}