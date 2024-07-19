using System.Data.SqlClient;

namespace PoliziaMunicipaleApp.Services
{
    public interface IDatabaseConnectionService
    {
        SqlConnection CreateConnection();
    }

    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private readonly string _connectionString;

        public DatabaseConnectionService(IConfiguration configuration)
        {
            // Mi Assicuro che la connessione non sia null
            _connectionString = configuration.GetConnectionString("Appconn")
                ?? throw new ArgumentNullException("La stringa di connessione non può essere nulla.");
        }

        public SqlConnection CreateConnection()
        {
            // Crea una nuova connessione con la stringa di connessione
            return new SqlConnection(_connectionString);
        }
    }
}
