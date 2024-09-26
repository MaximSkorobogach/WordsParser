namespace WordsParser.Infrastructure.Configurations;

internal class WordsParserSettings
{
    public required string RegexWordPattern { get; set; }
    public int MinFrequency { get; set; }
}