namespace WordsParser.Infrastructure.Repositories.Interfaces;

public interface IRepository<in T>
{
    Task AddOrUpdateAsync(T word);
}