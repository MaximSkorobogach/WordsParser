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
                    var configuration = context.Configuration;

                    RegisterConfigs(services, configuration);

                    RegisterDatabase(services, configuration);

                    RegisterServices(services);
                });

        private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new Exception("Не задана строка подключения к серверу ms sql");

            services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>(provider => new DatabaseInitializer(connectionString));

            services.AddScoped(provider => new DatabaseContext(connectionString));
            services.AddScoped<IRepository<Word>, WordsRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IWordsService, WordsService>();
            services.AddTransient<ITextFileService, TextFileService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IEndlessWordParserHandler, EndlessWordParserHandler>();
            services.AddTransient<IFileProcessingStrategy, FileProcessingStrategy>();
        }

        private static void RegisterConfigs(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IWordsParserSettings>(
                configuration.GetSection(nameof(WordsParserSettings)).Get<WordsParserSettings>()
                ?? throw new Exception("Не удалось обнаружить настройки для парсинга"));
            services.AddSingleton<IFileSettings>(
                configuration.GetSection(nameof(FileSettings)).Get<FileSettings>()
                ?? throw new Exception("Не удалось обнаружить настройки для обработки файла"));
        }
    }
}
