using WordsParser.Infrastructure.Consts;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Factories.Interfaces;
using WordsParser.Infrastructure.Repositories.Interfaces;

namespace WordsParser.Infrastructure.Repositories
{
    internal class WordsRepository(IDatabaseContextFactory databaseContextFactory) : IRepository<Word>
    {
        public async Task AddOrUpdateRangeAsync(IEnumerable<Word> words)
        {
            await using var dbContext = databaseContextFactory.CreateDbContext();

            await dbContext.ExecuteInTransactionAsync(async () =>
            {
                foreach (var word in words)
                {
                    var command = dbContext.CreateCommand(@$"
                        MERGE {Constantes.Database.WordsTable} AS target
                        USING (SELECT @word AS Word, @count AS Count) AS source
                        ON target.Word = source.Word
                        WHEN MATCHED THEN
                            UPDATE SET target.Count = target.Count + source.Count
                        WHEN NOT MATCHED THEN
                            INSERT (Word, Count) VALUES (source.Word, source.Count);");

                    command.Parameters.AddWithValue("@word", word.WordName);
                    command.Parameters.AddWithValue("@count", word.Count);

                    await command.ExecuteNonQueryAsync();
                }
            });
        }
    }
}