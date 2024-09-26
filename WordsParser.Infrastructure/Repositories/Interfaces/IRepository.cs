namespace WordsParser.Infrastructure.Repositories.Interfaces;

public interface IRepository<in T>
{
    Task AddOrUpdateRangeAsync(IEnumerable<T> entities);
}