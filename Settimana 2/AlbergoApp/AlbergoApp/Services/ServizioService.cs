using System.Collections.Generic;
using System.Data.SqlClient;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;

namespace AlbergoApp.Services
{
    public class ServizioService : IServizioService
    {
        private readonly IDatabaseService _databaseService;

        public ServizioService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreateServizioAsync(Servizio servizio)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "INSERT INTO Servizi (NomeServizio, Prezzo) OUTPUT INSERTED.IdServizio VALUES (@NomeServizio, @Prezzo)",
                    connection);
                command.Parameters.AddWithValue("@NomeServizio", servizio.NomeServizio ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);

                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
        }

        public async Task<Servizio?> GetServizioByIdAsync(int idServizio)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Servizi WHERE IdServizio = @IdServizio", connection);
                command.Parameters.AddWithValue("@IdServizio", idServizio);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Servizio
                        {
                            IdServizio = (int)reader["IdServizio"],
                            NomeServizio = reader["NomeServizio"].ToString(),
                            Prezzo = (decimal)reader["Prezzo"]
                        };
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Servizio>> GetAllServiziAsync()
        {
            var servizi = new List<Servizio>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Servizi", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        servizi.Add(new Servizio
                        {
                            IdServizio = (int)reader["IdServizio"],
                            NomeServizio = reader["NomeServizio"].ToString(),
                            Prezzo = (decimal)reader["Prezzo"]
                        });
                    }
                }
            }
            return servizi;
        }

        public async Task<bool> UpdateServizioAsync(Servizio servizio)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE Servizi SET NomeServizio = @NomeServizio, Prezzo = @Prezzo WHERE IdServizio = @IdServizio",
                    connection);
                command.Parameters.AddWithValue("@NomeServizio", servizio.NomeServizio ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);
                command.Parameters.AddWithValue("@IdServizio", servizio.IdServizio);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteServizioAsync(int idServizio)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Servizi WHERE IdServizio = @IdServizio", connection);
                command.Parameters.AddWithValue("@IdServizio", idServizio);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Servizio>> GetServiziByPrenotazioneIdAsync(int idPrenotazione)
        {
            var servizi = new List<Servizio>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT s.* FROM Servizi s JOIN ServiziPrenotazione sp ON s.IdServizio = sp.IdServizio WHERE sp.IdPrenotazione = @IdPrenotazione",
                    connection);
                command.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        servizi.Add(new Servizio
                        {
                            IdServizio = (int)reader["IdServizio"],
                            NomeServizio = reader["NomeServizio"].ToString(),
                            Prezzo = (decimal)reader["Prezzo"]
                        });
                    }
                }
            }
            return servizi;
        }

        public async Task<bool> AddServizioToPrenotazioneAsync(int idPrenotazione, int idServizio, DateTime dataServizio, int quantita, decimal prezzoUnitario, decimal prezzoTotale)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "INSERT INTO ServiziPrenotazione (IdPrenotazione, IdServizio, DataServizio, Quantita, PrezzoUnitario, PrezzoTotale) VALUES (@IdPrenotazione, @IdServizio, @DataServizio, @Quantita, @PrezzoUnitario, @PrezzoTotale)",
                    connection);
                command.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);
                command.Parameters.AddWithValue("@IdServizio", idServizio);
                command.Parameters.AddWithValue("@DataServizio", dataServizio);
                command.Parameters.AddWithValue("@Quantita", quantita);
                command.Parameters.AddWithValue("@PrezzoUnitario", prezzoUnitario);
                command.Parameters.AddWithValue("@PrezzoTotale", prezzoTotale);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> RemoveServizioFromPrenotazioneAsync(int idPrenotazione, int idServizio)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "DELETE FROM ServiziPrenotazione WHERE IdPrenotazione = @IdPrenotazione AND IdServizio = @IdServizio",
                    connection);
                command.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);
                command.Parameters.AddWithValue("@IdServizio", idServizio);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
