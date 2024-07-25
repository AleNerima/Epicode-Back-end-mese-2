using System.Data.SqlClient;
using AlbergoApp.Services.Interfaces;

namespace AlbergoApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDatabaseService _databaseService;

        public AccountService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool ValidateUser(string username, string password, out string role)
        {
            role = null;
            using (var connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT PasswordHash, Ruolo FROM Dipendenti WHERE Username = @Username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPasswordHash = reader["PasswordHash"].ToString();
                            role = reader["Ruolo"].ToString();
                            if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void RegisterUser(string username, string password, string nome, string cognome, string role)
        {
            using (var connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Dipendenti (Username, PasswordHash, Nome, Cognome, Ruolo) VALUES (@Username, @PasswordHash, @Nome, @Cognome, @Ruolo)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(password));
                    command.Parameters.AddWithValue("@Nome", nome);
                    command.Parameters.AddWithValue("@Cognome", cognome);
                    command.Parameters.AddWithValue("@Ruolo", role); 

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
