using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.Consts;
using WordsParser.Infrastructure.Database;
using WordsParser.Infrastructure.Database.Interfaces;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Handlers;
using WordsParser.Infrastructure.Handlers.Interfaces;
using WordsParser.Infrastructure.Repositories;
using WordsParser.Infrastructure.Repositories.Interfaces;
using WordsParser.Infrastructure.Services;
using WordsParser.Infrastructure.Services.Interfaces;
using WordsParser.Infrastructure.Strategies;
using WordsParser.Infrastructure.Strategies.Interfaces;

namespace WordsParser.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder().Build();

            await InitializeDatabaseAsync(host);

            await host.Services.GetRequiredService<IEndlessWordParserHandler>().StartHandlingAsync();
        }


        private static async Task InitializeDatabaseAsync(IHost host)
        {
            await host.Services.GetRequiredService<IDatabaseInitializer>().InitializeDatabaseAsync();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                                           ?? throw new Exception("Не задана строка подключения к серверу ms sql");
                    
                    RegisterConfigs(services, context);

                    services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>(provider => new DatabaseInitializer(connectionString));
                    services.AddTransient(provider => new DatabaseContext(connectionString));

                    services.AddTransient<IRepository<Word>, WordsRepository>();
                    services.AddTransient<IWordsService, WordsService>();
                    services.AddTransient<ITextFileService, TextFileService>();
                    services.AddTransient<IEndlessWordParserHandler, EndlessWordParserHandler>();
                    services.AddTransient<IFileProcessingStrategy, FileProcessingStrategy>();
                });

        private static void RegisterConfigs(IServiceCollection services, HostBuilderContext context)
        {
            services.AddSingleton<IWordsParserSettings>(
                context.Configuration.GetSection(nameof(WordsParserSettings)).Get<WordsParserSettings>()
                ?? throw new Exception("Не удалось обнаружить настройки для парсинга"));
            services.AddSingleton<IFileSettings>(
                context.Configuration.GetSection(nameof(FileSettings)).Get<FileSettings>()
                ?? throw new Exception("Не удалось обнаружить настройки для обработки файла"));
        }
    }
}
