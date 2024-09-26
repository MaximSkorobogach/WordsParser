using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

internal class FileService : IFileService
{
    public void ThrowIfFileNotExists(string? filePath)
    {
        if (filePath is null || !File.Exists(filePath))
            throw new FileNotFoundException("Путь к файлу не доступен.");
    }

    public decimal ConvertBytesSizeToMbytesSize(long bytes) => bytes / (decimal)( 1024.0 * 1024.0 );
}