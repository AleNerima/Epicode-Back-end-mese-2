using System.Data;
using System.Data.SqlClient;

namespace DittaSpedizioniApp.Services
{
    public class DatabaseConnection : IDisposable
    {
        private readonly string? _connectionString;
        private SqlConnection? _connection;

        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppConn");
        }

        public IDbConnection GetOpenConnection()
        {
            _connection = new SqlConnection(_connectionString);

            try
            {
                _connection.Open();
                return _connection;
            }
            catch (SqlException ex)
            {
                // Gestisci l'eccezione o rilanciala se necessario
                // Esempio di logging dell'eccezione
                Console.WriteLine($"Errore durante l'apertura della connessione: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
