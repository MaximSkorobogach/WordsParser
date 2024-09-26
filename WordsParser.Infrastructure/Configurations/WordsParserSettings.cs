using WordsParser.Infrastructure.Configurations.Interfaces;

namespace WordsParser.Infrastructure.Configurations;

public class WordsParserSettings : IWordsParserSettings
{
    public required string RegexWordPattern { get; set; }
    public int MinFrequency { get; set; }
}