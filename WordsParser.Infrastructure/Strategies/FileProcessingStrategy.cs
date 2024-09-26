using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Exceptions;
using WordsParser.Infrastructure.Services.Interfaces;
using WordsParser.Infrastructure.Strategies.Interfaces;

namespace WordsParser.Infrastructure.Strategies;

internal class FileProcessingStrategy(IOptions<FileSettings> fileSettings, 
    ITextFileService textFileService, IFileService fileService, IWordsService wordsService,
    ILogger<IFileProcessingStrategy> logger) : IFileProcessingStrategy
{
    public async Task ExecuteAsync(string? filePath)
    {
        try
        {
            fileService.ThrowIfFileNotExists(filePath);

            var fileInfo = new FileInfo(filePath!);

            var fileMbytesSize = fileService.ConvertBytesSizeToMbytesSize(fileInfo.Length);

            if (fileMbytesSize > fileSettings.Value.MaxFileSizeMbytes)
                throw new Exception("Файл превышает лимит в 1000 МБ.");

            var wordsCountMap = textFileService.GetWords(filePath);

            if (!wordsCountMap.Any())
            {
                logger.LogInformation("Слов к сохранению нет.");
                return;
            }

            CreateWordsLog(wordsCountMap);

            await wordsService.SaveWordsCountAsync(wordsCountMap);
        }
        catch (Exception e)
        {
            throw new StrategyException($"Не удалось обработать стратегию {nameof(FileProcessingStrategy)}, message : {e.Message}");
        }
    }

    private void CreateWordsLog(List<Word> wordsCountMap)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("К сохранению идут следующие слова:");

        foreach (var word in wordsCountMap) 
            stringBuilder.AppendLine($"Слово {word.WordName} - кол-во {word.Count}");

        logger.LogInformation(stringBuilder.ToString());
    }
}