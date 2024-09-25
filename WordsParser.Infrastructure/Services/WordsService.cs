using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Repositories;
using WordsParser.Infrastructure.Repositories.Interfaces;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

public class WordsService(IRepository<Word> wordsRepository) : IWordsService
{
    public async Task SaveWordsCountAsync(List<Word> words)
    {
        foreach (var word in words)
            await wordsRepository.AddOrUpdateAsync(word);
    }
}