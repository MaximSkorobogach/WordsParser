using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.Database;
using WordsParser.Infrastructure.Database.Interfaces;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Repositories;
using WordsParser.Infrastructure.Repositories.Interfaces;
using WordsParser.Infrastructure.Services;
using WordsParser.Infrastructure.Services.Interfaces;

namespace WordsParser.ConsoleApp
{
    internal class Program
    {
        async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await InitializeDatabaseAsync(host);
        }

        private async Task InitializeDatabaseAsync(IHost host)
        {
            await host.Services.GetRequiredService<DatabaseInitializer>().InitializeDatabaseAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IWordsParserSettings>(
                        context.Configuration.GetSection(nameof(WordsParserSettings)).Get<WordsParserSettings>()
                        ?? throw new InvalidOperationException());

                    services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();

                    services.AddTransient(provider =>
                    {
                        var configuration = provider.GetRequiredService<IConfiguration>();
                        var connectionString = configuration.GetConnectionString("DefaultConnection")
                                               ?? throw new Exception("Не задана строка подключения к серверу ms sql");
                        return new DatabaseContext(connectionString);
                    });


                    services.AddTransient<IRepository<Word>, WordsRepository>(); 
                    services.AddTransient<IWordsService, WordsService>(); 
                    services.AddTransient<ITextFileService, TextFileService>(); 
                });
    }
}
