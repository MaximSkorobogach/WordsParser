namespace WordsParser.Infrastructure.Strategies.Interfaces;

public interface IFileProcessingStrategy
{
    Task TryExecuteAsync(string? filePath);
}