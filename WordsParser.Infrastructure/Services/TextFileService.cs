using System.Text;
using System.Text.RegularExpressions;
using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

public class TextFileService(IWordsParserSettings wordsParserSettings) : ITextFileService
{
    public void ThrowIfFileNotExists(string? filePath)
    {
        if (filePath is null || !File.Exists(filePath))
            throw new Exception("Путь к файлу не доступен.");
    }

    public List<Word> GetWords(string? filePath)
    {
        ThrowIfFileNotExists(filePath);

        var fileContent = File.ReadAllText(filePath!, Encoding.UTF8);

        var matches = 
            Regex.Matches(fileContent, wordsParserSettings.RegexWordPattern, 
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        var filteredWords = 
            matches
                .Where(word => word.Length >= wordsParserSettings.MinFrequency)
                .Select(match => match.Value.ToLowerInvariant());

        var words = 
            filteredWords
                .GroupBy(word => word)
                .Select(word => new Word(word.Key, word.Count()))
                .ToList();

        return words;
    }
}