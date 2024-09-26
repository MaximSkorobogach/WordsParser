using WordsParser.Infrastructure.DTO;

namespace WordsParser.Infrastructure.Services.Interfaces;

public interface ITextFileService
{
    List<Word> GetWords(string? filePath);
}