using System.Data.SqlClient;
using AlbergoApp.Services.Interfaces;


public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Appconn")
            ?? throw new ArgumentNullException(nameof(configuration), "Stringa di connessione non trovata");
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
