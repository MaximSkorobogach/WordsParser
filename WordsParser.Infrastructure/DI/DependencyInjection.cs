using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WordsParser.Infrastructure.Configurations.Interfaces;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.Database.Interfaces;
using WordsParser.Infrastructure.Database;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Handlers.Interfaces;
using WordsParser.Infrastructure.Handlers;
using WordsParser.Infrastructure.Repositories.Interfaces;
using WordsParser.Infrastructure.Repositories;
using WordsParser.Infrastructure.Services.Interfaces;
using WordsParser.Infrastructure.Services;
using WordsParser.Infrastructure.Strategies.Interfaces;
using WordsParser.Infrastructure.Strategies;

namespace WordsParser.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection
            .RegisterLogger()
            .RegisterConfigs(configuration)
            .RegisterDatabase(configuration)
            .RegisterServices();
    }

    private static IServiceCollection RegisterLogger(this IServiceCollection services)
    {
        return services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddConsole();
        });
    }

    private static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new Exception("Не задана строка подключения к серверу ms sql");

        return services
            .AddSingleton<IDatabaseInitializer, DatabaseInitializer>(provider => new DatabaseInitializer(connectionString))
            .AddScoped(provider => new DatabaseContext(connectionString))
            .AddScoped<IRepository<Word>, WordsRepository>();
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IWordsService, WordsService>()
            .AddTransient<ITextFileService, TextFileService>()
            .AddTransient<IFileService, FileService>()
            .AddTransient<IEndlessWordParserHandler, EndlessWordParserHandler>()
            .AddTransient<IFileProcessingStrategy, FileProcessingStrategy>();
    }

    private static IServiceCollection RegisterConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<IWordsParserSettings>(
            configuration.GetSection(nameof(WordsParserSettings)).Get<WordsParserSettings>()
            ?? throw new Exception("Не удалось обнаружить настройки для парсинга"))
            .AddSingleton<IFileSettings>(
            configuration.GetSection(nameof(FileSettings)).Get<FileSettings>()
            ?? throw new Exception("Не удалось обнаружить настройки для обработки файла"));
    }
}