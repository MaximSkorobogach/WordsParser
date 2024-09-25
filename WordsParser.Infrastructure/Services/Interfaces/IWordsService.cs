namespace WordsParser.Infrastructure.Services.Interfaces;

public interface IWordsService
{
    Task SaveWordsCountAsync(Dictionary<string, int> wordsCountMap);
}