using System.Data.Common;
using System.Data.SqlClient;

namespace WordsParser.Infrastructure.Database
{
    public class DatabaseContext(string connectionString) : IDisposable
    {
        private readonly SqlConnection _connection = new(connectionString);

        public async Task OpenConnectionAsync()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }
        }

        public SqlCommand CreateCommand(string commandText)
        {
            return new SqlCommand(commandText, _connection);
        }

        public void Dispose()
        {
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}