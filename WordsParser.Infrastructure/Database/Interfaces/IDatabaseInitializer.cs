namespace WordsParser.Infrastructure.Database.Interfaces;

public interface IDatabaseInitializer
{
    Task InitializeDatabaseAsync();
}