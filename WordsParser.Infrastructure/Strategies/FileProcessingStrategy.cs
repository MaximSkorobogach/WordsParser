using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.Services.Interfaces;
using WordsParser.Infrastructure.Strategies.Interfaces;

namespace WordsParser.Infrastructure.Strategies;

public class FileProcessingStrategy(IFileSettings fileSettings, 
    ITextFileService textFileService, IFileService fileService, IWordsService wordsService) : IFileProcessingStrategy
{
    public async Task TryExecuteAsync(string? filePath)
    {
        try
        {
            fileService.ThrowIfFileNotExists(filePath);

            var fileInfo = new FileInfo(filePath!);

            var fileMbytesSize = fileService.ConvertBytesSizeToMbytesSize(fileInfo.Length);

            if (fileMbytesSize > fileSettings.MaxFileSizeMbytes)
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