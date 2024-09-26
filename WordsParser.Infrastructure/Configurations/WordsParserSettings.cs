namespace WordsParser.Infrastructure.Configurations;

public class WordsParserSettings
{
    public required string RegexWordPattern { get; set; }
    public int MinFrequency { get; set; }
}