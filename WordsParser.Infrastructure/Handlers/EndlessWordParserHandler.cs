using WordsParser.Infrastructure.Consts;
using WordsParser.Infrastructure.Handlers.Interfaces;
using WordsParser.Infrastructure.Strategies.Interfaces;

namespace WordsParser.Infrastructure.Handlers;

public class EndlessWordParserHandler(IFileProcessingStrategy fileProcessingStrategy) : IEndlessWordParserHandler
{
    public async Task StartHandlingAsync()
    {
        while (true)
        {
            Console.WriteLine("Введите путь к текстовому файлу (или 'exit' для выхода):");

            var input = Console.ReadLine();

            if (input?.Trim().ToLower() == Constantes.ConsoleCommands.ExitCommand)
            {
                break;
            }

            try
            {
                await fileProcessingStrategy.TryExecuteAsync(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}