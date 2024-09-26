using Microsoft.Extensions.Logging;
using WordsParser.Infrastructure.Consts;
using WordsParser.Infrastructure.Handlers.Interfaces;
using WordsParser.Infrastructure.Strategies.Interfaces;

namespace WordsParser.Infrastructure.Handlers;

internal class EndlessWordParserHandler(IFileProcessingStrategy fileProcessingStrategy, 
    ILogger<IEndlessWordParserHandler> logger) : IEndlessWordParserHandler
{
    public async Task TryStartHandlingAsync()
    {
        while (true)
        {
            Console.WriteLine("Введите путь к текстовому файлу (или 'exit' для выхода):");

            var input = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(input)) continue;

            if (input?.Trim().ToLower() == Constantes.ConsoleCommands.ExitCommand) break;

            try
            {
                await fileProcessingStrategy.ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Возникла ошибка обработки");
            }
        }
    }
}