namespace Shared.WPF.Services.Conversion;

public interface IDocumentConversionService
{
    /// <summary>
    /// Конвертирует список изображений в один PDF файл
    /// </summary>
    Task<byte[]> ConvertImagesToPdfAsync
            (
                string documentType, 
                string employeeId,
                List<byte[]> files,
                List<string>? fileNames = null,
                CancellationToken cancellationToken = default
            );
}