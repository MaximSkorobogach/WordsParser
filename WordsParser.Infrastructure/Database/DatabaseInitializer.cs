using System.Data.SqlClient;
using WordsParser.Infrastructure.Consts;
using WordsParser.Infrastructure.Database.Interfaces;

namespace WordsParser.Infrastructure.Database
{
    public class DatabaseInitializer(string connectionString) : IDatabaseInitializer
    {
        public async Task InitializeDatabaseAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@$"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{Constantes.Database.WordsDatabase}')
                    BEGIN
                        CREATE DATABASE WordsParserDB;
                    END
                    USE WordsParserDB;
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Words]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE Words (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Word NVARCHAR(20) NOT NULL,
                            Count INT NOT NULL DEFAULT 0
                        );
                    END", connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}