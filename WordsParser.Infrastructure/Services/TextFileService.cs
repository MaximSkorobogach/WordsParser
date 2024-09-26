using System.Text;
using System.Text.RegularExpressions;
using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

public class TextFileService(IWordsParserSettings wordsParserSettings, IFileService filesService) : ITextFileService
{
    public List<Word> GetWords(string? filePath)
    {
        filesService.ThrowIfFileNotExists(filePath);

        var fileContent = File.ReadAllText(filePath!, Encoding.UTF8);

        var matches = 
            Regex.Matches(fileContent, wordsParserSettings.RegexWordPattern, 
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        var words = 
            matches
                .Where(word => word.Length >= wordsParserSettings.MinFrequency)
                .GroupBy(word => word.Value.ToLowerInvariant())
                .Select(word => new Word(word.Key, word.Count()))
                .ToList();

        return words;
    }
}