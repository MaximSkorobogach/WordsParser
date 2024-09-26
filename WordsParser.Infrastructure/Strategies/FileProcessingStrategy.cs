using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.Services.Interfaces;
using WordsParser.Infrastructure.Strategies.Interfaces;

namespace WordsParser.Infrastructure.Strategies;

public class FileProcessingStrategy(IFileSettings fileSettings, 
    ITextFileService textFileService, IWordsService wordsService) : IFileProcessingStrategy
{
    public async Task TryExecuteAsync(string? filePath)
    {
        try
        {
            textFileService.ThrowIfFileNotExists(filePath);

            var fileInfo = new FileInfo(filePath!);

            if (fileInfo.Length > fileSettings.MaxFileSizeMbytes)
                throw new Exception("Файл превышает лимит в 1000 МБ.");

            var wordsCountMap = textFileService.GetWords(filePath);

            await wordsService.SaveWordsCountAsync(wordsCountMap);
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось обработать файл, message : {e.Message}");
        }
    }
}