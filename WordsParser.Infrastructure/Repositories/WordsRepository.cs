using System.Data.SqlClient;
using System.Threading.Tasks;
using WordsParser.Infrastructure.Consts;
using WordsParser.Infrastructure.Database;

namespace WordsParser.Infrastructure.Repositories
{
    public class WordsRepository(DatabaseContext context)
    {
        public async Task AddOrUpdateWordAsync(string word, int count)
        {
            await context.OpenConnectionAsync();

            var command = context.CreateCommand(@$"
                IF EXISTS (SELECT * FROM {Constantes.Database.WordsTable} WHERE Word = @word)
                BEGIN
                    UPDATE Words SET Count = Count + @count WHERE Word = @word;
                END
                ELSE
                BEGIN
                    INSERT INTO Words (Word, Count) VALUES (@word, @count);
                END");

            command.Parameters.AddWithValue("@word", word);
            command.Parameters.AddWithValue("@count", count);

            await command.ExecuteNonQueryAsync();
        }
    }
}