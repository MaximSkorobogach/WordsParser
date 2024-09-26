namespace WordsParser.Infrastructure.Services.Interfaces;

public interface IFileService
{
    void ThrowIfFileNotExists(string? filePath);
    decimal ConvertBytesSizeToMbytesSize(long bytes);
}