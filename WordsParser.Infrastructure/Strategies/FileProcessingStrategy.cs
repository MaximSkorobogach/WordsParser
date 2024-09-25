using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.Services.Interfaces;
using WordsParser.Infrastructure.Strategies.Interfaces;

namespace WordsParser.Infrastructure.Strategies;

public class FileProcessingStrategy(IFileSettings fileSettings, 
    ITextFileService textFileService, IWordsService wordsService) : IFileProcessingStrategy
{
    public async Task ExecuteAsync(string? filePath)
    {
        if (filePath is null || !File.Exists(filePath))
            throw new Exception("Путь к файлу не доступен.");

        var fileInfo = new FileInfo(filePath);

        if (fileInfo.Length > fileSettings.MaxFileSizeMbytes)
            throw new Exception("Файл превышает лимит в 1000 МБ.");

        var wordsCountMap = textFileService.GetWordsCountMap(filePath);

        await wordsService.SaveWordsCountAsync(wordsCountMap);
    }
}