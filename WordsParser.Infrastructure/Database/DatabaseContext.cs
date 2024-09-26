using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace WordsParser.Infrastructure.Database
{
    public class DatabaseContext(string connectionString, ILogger<DatabaseContext> logger) : IAsyncDisposable
    {
        private readonly SqlConnection _connection = new(connectionString);
        private DbTransaction? _transaction;

        private async Task OpenConnectionAsync()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.ConnectionString = connectionString;
                await _connection.OpenAsync();
                logger.LogInformation($"Открыто соединение с бд, connectionString - {connectionString}");
            }
        }

        public SqlCommand CreateCommand(string commandText)
        {
            return 
                _transaction == null 
                    ? new SqlCommand(commandText, _connection) 
                    : new SqlCommand(commandText, _connection, (SqlTransaction)_transaction);
        }

        private async Task<DbTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Транзакция уже активна. Завершите текущую транзакцию перед началом новой.");
            }

            _transaction = await _connection.BeginTransactionAsync(IsolationLevel.Serializable);
            return _transaction;
        }

        private async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Нет активной транзакции для коммита.");
            }

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        private void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await OpenConnectionAsync();
            await using var transaction = await BeginTransactionAsync();

            try
            {
                await action();
                await CommitTransactionAsync();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
            GC.SuppressFinalize(this);
            logger.LogInformation($"Закрыто соединение с БД, connectionString - {connectionString}");
        }
    }
}
