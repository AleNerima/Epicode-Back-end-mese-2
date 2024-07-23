using System.Data.SqlClient;
using AlbergoApp.Services.Interfaces;


namespace AlbergoApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDatabaseService  _databaseService;

        public AccountService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool ValidateUser(string username, string password)
        {
            using (var connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT PasswordHash FROM Dipendenti WHERE Username = @Username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        string storedPasswordHash = result.ToString();
                        return BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
                    }
                }
            }
            return false;
        }

        public void RegisterUser(string username, string password, string nome, string cognome)
        {
            using (var connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Dipendenti (Username, PasswordHash, Nome, Cognome) VALUES (@Username, @PasswordHash, @Nome, @Cognome)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(password));
                    command.Parameters.AddWithValue("@Nome", nome);
                    command.Parameters.AddWithValue("@Cognome", cognome);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
