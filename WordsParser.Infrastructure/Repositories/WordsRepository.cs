using System.Data.SqlClient;
using System.Threading.Tasks;
using WordsParser.Infrastructure.Consts;
using WordsParser.Infrastructure.Database;
using WordsParser.Infrastructure.DTO;
using WordsParser.Infrastructure.Repositories.Interfaces;

namespace WordsParser.Infrastructure.Repositories
{
    public class WordsRepository(DatabaseContext context) : IRepository<Word>
    {
        public async Task AddOrUpdateAsync(Word word)
        {
            await context.OpenConnectionAsync();

            var command = context.CreateCommand(@$"
                IF EXISTS (SELECT 1 FROM {Constantes.Database.WordsTable} WHERE Word = @word)
                BEGIN
                    UPDATE Words SET Count = Count + @count WHERE Word = @word;
                END
                ELSE
                BEGIN
                    INSERT INTO Words (Word, Count) VALUES (@word, @count);
                END");

            command.Parameters.AddWithValue("@word", word.WordName);
            command.Parameters.AddWithValue("@count", word.Count);

            await command.ExecuteNonQueryAsync();
        }
    }
}