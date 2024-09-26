using Microsoft.Extensions.Options;
using System.Text;
using System.Text.RegularExpressions;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

internal class TextFileService(IOptions<WordsParserSettings> wordsParserSettings, IFileService filesService) : ITextFileService
{
    public List<Word> GetWords(string? filePath)
    {
        filesService.ThrowIfFileNotExists(filePath);

        var fileContent = File.ReadAllText(filePath!, Encoding.UTF8);

        var matches = 
            Regex.Matches(fileContent, wordsParserSettings.Value.RegexWordPattern, 
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        var words = 
            matches
                .Where(word => word.Length >= wordsParserSettings.Value.MinFrequency)
                .GroupBy(word => word.Value.ToLowerInvariant())
                .Select(word => new Word(word.Key, word.Count()))
                .ToList();

        return words;
    }
}