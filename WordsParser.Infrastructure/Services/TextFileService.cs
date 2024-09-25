using System.Text;
using System.Text.RegularExpressions;
using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.Infrastructure.Services;

public class TextFileService(IWordsParserSettings wordsParserSettings) : ITextFileService
{
    public Dictionary<string, int> GetWordsCountMap(string filePath)
    {
        var words = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var fileContent = File.ReadAllText(filePath, Encoding.UTF8);

        var matches = 
            Regex.Matches(fileContent, wordsParserSettings.RegexWordPattern, 
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        foreach (Match match in matches)
        {
            var word = match.Value.ToLowerInvariant();

            if (word.Length < wordsParserSettings.MinFrequency) continue;

            if (!words.TryAdd(word, 1))
            {
                words[word]++;
            }
        }

        return words;
    }
}