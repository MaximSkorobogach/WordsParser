namespace WordsParser.Infrastructure.Strategies.Interfaces;

public interface IFileProcessingStrategy
{
    Task ProcessFileAsync(string filePath);
}