using Microsoft.Extensions.FileProviders;

namespace WordsParser.Infrastructure.Services.Interfaces;

public interface IFileService
{
    void ThrowIfFileNotExists(string? filePath);
    decimal GetFileMBytesSize(FileInfo fileInfo);
}