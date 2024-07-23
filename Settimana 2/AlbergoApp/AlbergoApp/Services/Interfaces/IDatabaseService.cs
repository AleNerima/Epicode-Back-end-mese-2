using System.Data.SqlClient;

namespace AlbergoApp.Services.Interfaces
{
    public interface IDatabaseService
    {
        SqlConnection GetConnection();
    }
}
