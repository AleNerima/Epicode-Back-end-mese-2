using System.Data.SqlClient;


namespace DittaSpedizioni.Services
{
    public class DatabaseConnection
    {
        private readonly string? _connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Appconn");
        } 

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
