using WordsParser.Infrastructure.Database;

namespace WordsParser.Infrastructure.Factories.Interfaces;

public interface IDatabaseContextFactory
{
    DatabaseContext CreateDbContext();
}