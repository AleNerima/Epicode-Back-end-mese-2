using System.Data.SqlClient;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;

namespace DittaSpedizioni.Services
{
    public class UtenteService : IUtenteService
    {
        private readonly DatabaseConnection _databaseConnection;

        public UtenteService(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public Utente GetUtenteByEmail(string email)
        {
            Utente utente = null;

            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Utenti WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    utente = new Utente
                    {
                        IdUtente = (int)reader["IdUtente"],
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Ruolo = reader["Ruolo"].ToString()
                    };
                }
            }

            return utente;
        }

        public void AddUtente(Utente utente)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "INSERT INTO Utenti (Nome, Cognome, Email, PasswordHash, Ruolo) VALUES (@Nome, @Cognome, @Email, @PasswordHash, @Ruolo)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", utente.Nome);
                cmd.Parameters.AddWithValue("@Cognome", utente.Cognome);
                cmd.Parameters.AddWithValue("@Email", utente.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", utente.PasswordHash);
                cmd.Parameters.AddWithValue("@Ruolo", utente.Ruolo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
