namespace WordsParser.Infrastructure.Strategies.Interfaces;

public interface IFileProcessingStrategy
{
    Task ExecuteAsync(string? filePath);
}