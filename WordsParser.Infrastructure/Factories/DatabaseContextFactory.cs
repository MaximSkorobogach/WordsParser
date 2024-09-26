using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WordsParser.Infrastructure.Configurations;
using WordsParser.Infrastructure.Database;
using WordsParser.Infrastructure.Factories.Interfaces;

namespace WordsParser.Infrastructure.Factories;

internal class DatabaseContextFactory(IOptions<DatabaseConnectionOptions> databaseOptions, ILogger<DatabaseContext> logger) : IDatabaseContextFactory
{
    public DatabaseContext CreateDbContext()
    {
        return new DatabaseContext(databaseOptions.Value.DefaultConnection, logger);
    }
}