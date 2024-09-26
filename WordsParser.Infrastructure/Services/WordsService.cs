using System.Diagnostics;
using Microsoft.Extensions.Logging;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Repositories.Interfaces;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

internal class WordsService(IRepository<Word> wordsRepository, ILogger<IWordsService> logger) : IWordsService
{
    public async Task SaveWordsCountAsync(List<Word> words)
    {
        logger.LogInformation($"Начало сохранения {words.Count} слов");
        var stopWatch = new Stopwatch();

        stopWatch.Start();

        await wordsRepository.AddOrUpdateRangeAsync(words);

        stopWatch.Stop();
        logger.LogInformation($"Сохранение {words.Count} слов завершено, кол-во затраченного времени {stopWatch.Elapsed}");
    }
}