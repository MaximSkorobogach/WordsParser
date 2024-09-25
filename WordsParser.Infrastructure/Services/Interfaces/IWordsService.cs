using WordsParser.Infrastructure.DTO;

namespace WordsParser.Infrastructure.Services.Interfaces;

public interface IWordsService
{
    Task SaveWordsCountAsync(List<Word> words);
}