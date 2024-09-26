using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.Database.Interfaces;
using WordsParser.Infrastructure.Database;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Factories;
using WordsParser.Infrastructure.Factories.Interfaces;
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
        return services
            .AddSingleton<IDatabaseInitializer, DatabaseInitializer>()
            .AddScoped<IDatabaseContextFactory, DatabaseContextFactory>()
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
            .Configure<DatabaseConnectionOptions>(configuration.GetSection("ConnectionStrings"))
            .Configure<WordsParserSettings>(configuration.GetSection(nameof(WordsParserSettings)))
            .Configure<FileSettings>(configuration.GetSection(nameof(FileSettings)));
    }
}