namespace WordsParser.Infrastructure.Configurations.Interfaces;

public interface IWordsParserSettings
{
    string RegexWordPattern { get; set; }
    int MinFrequency { get; set; }
}