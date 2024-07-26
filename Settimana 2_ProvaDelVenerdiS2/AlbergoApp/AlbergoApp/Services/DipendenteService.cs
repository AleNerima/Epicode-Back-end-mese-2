using System.Data.SqlClient;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;

namespace AlbergoApp.Services
{
    public class DipendenteService : IDipendenteService
    {
        private readonly IDatabaseService _databaseService;

        public DipendenteService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreateDipendenteAsync(Dipendente dipendente)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Dipendenti (Username, PasswordHash, Nome, Cognome, Ruolo) OUTPUT INSERTED.IdDipendente VALUES (@Username, @PasswordHash, @Nome, @Cognome, @Ruolo)", connection);
                command.Parameters.AddWithValue("@Username", (object)dipendente.Username ?? DBNull.Value);
                command.Parameters.AddWithValue("@PasswordHash", (object)dipendente.PasswordHash ?? DBNull.Value);
                command.Parameters.AddWithValue("@Nome", (object)dipendente.Nome ?? DBNull.Value);
                command.Parameters.AddWithValue("@Cognome", (object)dipendente.Cognome ?? DBNull.Value);
                command.Parameters.AddWithValue("@Ruolo", (object)dipendente.Ruolo ?? DBNull.Value);

                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
        }

        public async Task<Dipendente?> GetDipendenteByIdAsync(int idDipendente)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Dipendenti WHERE IdDipendente = @IdDipendente", connection);
                command.Parameters.AddWithValue("@IdDipendente", idDipendente);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Dipendente
                        {
                            IdDipendente = (int)reader["IdDipendente"],
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),
                            Ruolo = reader["Ruolo"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Dipendente>> GetAllDipendentiAsync()
        {
            var dipendenti = new List<Dipendente>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Dipendenti", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        dipendenti.Add(new Dipendente
                        {
                            IdDipendente = (int)reader["IdDipendente"],
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),
                            Ruolo = reader["Ruolo"].ToString()
                        });
                    }
                }
            }
            return dipendenti;
        }

        public async Task<bool> UpdateDipendenteAsync(Dipendente dipendente)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Dipendenti SET Username = @Username, PasswordHash = @PasswordHash, Nome = @Nome, Cognome = @Cognome, Ruolo = @Ruolo WHERE IdDipendente = @IdDipendente", connection);
                command.Parameters.AddWithValue("@Username", (object)dipendente.Username ?? DBNull.Value);
                command.Parameters.AddWithValue("@PasswordHash", (object)dipendente.PasswordHash ?? DBNull.Value);
                command.Parameters.AddWithValue("@Nome", (object)dipendente.Nome ?? DBNull.Value);
                command.Parameters.AddWithValue("@Cognome", (object)dipendente.Cognome ?? DBNull.Value);
                command.Parameters.AddWithValue("@Ruolo", (object)dipendente.Ruolo ?? DBNull.Value);
                command.Parameters.AddWithValue("@IdDipendente", dipendente.IdDipendente);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteDipendenteAsync(int idDipendente)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Dipendenti WHERE IdDipendente = @IdDipendente", connection);
                command.Parameters.AddWithValue("@IdDipendente", idDipendente);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<Dipendente?> GetDipendenteByUsernameAsync(string username)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Dipendenti WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Dipendente
                        {
                            IdDipendente = (int)reader["IdDipendente"],
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),
                            Ruolo = reader["Ruolo"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}
