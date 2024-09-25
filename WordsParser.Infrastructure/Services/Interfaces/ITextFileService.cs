namespace WordsParser.Infrastructure.Services.Interfaces;

public interface ITextFileService
{
    Dictionary<string, int> GetWordsCountMap(string filePath);
}