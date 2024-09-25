using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Repositories;
using WordsParser.Infrastructure.Repositories.Interfaces;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

public class WordsService(IRepository<Word> wordsRepository) : IWordsService
{
    public async Task SaveWordsCountAsync(Dictionary<string, int> wordsCountMap)
    {
        foreach (var word in wordsCountMap.Select(map => new Word(map.Key, map.Value)))
            await wordsRepository.AddOrUpdateAsync(word);
    }
}