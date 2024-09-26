namespace WordsParser.Infrastructure.Handlers.Interfaces;

public interface IEndlessWordParserHandler
{
    Task TryStartHandlingAsync();
}