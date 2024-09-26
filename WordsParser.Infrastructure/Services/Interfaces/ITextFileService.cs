using WordsParser.Infrastructure.DTO;

namespace WordsParser.Infrastructure.Services.Interfaces;

public interface ITextFileService
{
    void ThrowIfFileNotExists(string? filePath);
    List<Word> GetWords(string? filePath);
}