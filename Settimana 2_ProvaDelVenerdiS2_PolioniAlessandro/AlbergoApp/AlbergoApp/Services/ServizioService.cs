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
            try
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
            catch (SqlException ex)
            {                
                throw new ApplicationException("Database error occurred while creating the service.", ex);
            }
        }
                
        public async Task<Servizio?> GetServizioByIdAsync(int idServizio)
        {
            try
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
            }
            catch (SqlException ex)
            {
                
                throw new ApplicationException("Database error occurred while retrieving the service.", ex);
            }
            return null;
        }

        
        public async Task<IEnumerable<Servizio>> GetAllServiziAsync()
        {
            var servizi = new List<Servizio>();

            try
            {
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
            }
            catch (SqlException ex)
            {
                
                throw new ApplicationException("Database error occurred while retrieving all services.", ex);
            }
            return servizi;
        }

        
        public async Task<bool> UpdateServizioAsync(Servizio servizio)
        {
            try
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
            catch (SqlException ex)
            {
                
                throw new ApplicationException("Database error occurred while updating the service.", ex);
            }
        }

        
        public async Task<bool> DeleteServizioAsync(int idServizio)
        {
            try
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
            catch (SqlException ex)
            {
                
                throw new ApplicationException("Database error occurred while deleting the service.", ex);
            }
        }

        // Metodo per ottenere tutti i servizi prenotati associati a una prenotazione
        public async Task<IEnumerable<ServiziPrenotazione>> GetServiziPrenotatiByPrenotazioneIdAsync(int prenotazioneId)
        {
            var serviziPrenotati = new List<ServiziPrenotazione>();

            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand(
                        @"SELECT sp.*, s.NomeServizio
                        FROM ServiziPrenotazione sp
                        INNER JOIN Servizi s ON sp.IdServizio = s.IdServizio
                        WHERE sp.IdPrenotazione = @IdPrenotazione",
                        connection);
                    command.Parameters.AddWithValue("@IdPrenotazione", prenotazioneId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            serviziPrenotati.Add(new ServiziPrenotazione
                            {
                                IdPrenotazione = (int)reader["IdPrenotazione"],
                                IdServizio = (int)reader["IdServizio"],
                                DataServizio = (DateTime)reader["DataServizio"],
                                Quantita = (int)reader["Quantita"],
                                PrezzoUnitario = (decimal)reader["PrezzoUnitario"],
                                Servizio = new Servizio
                                {
                                    NomeServizio = reader["NomeServizio"].ToString()
                                }
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                
                throw new ApplicationException("Errore Database", ex);
            }

            return serviziPrenotati.AsEnumerable(); // Restituisce IEnumerable<ServiziPrenotazione>
        }


        // Metodo per aggiungere un servizio a una prenotazione
        public async Task<bool> AddServizioToPrenotazioneAsync(int idPrenotazione, int idServizio, DateTime dataServizio, int quantita, decimal prezzoUnitario)
        {
            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand(
                        "INSERT INTO ServiziPrenotazione (IdPrenotazione, IdServizio, DataServizio, Quantita, PrezzoUnitario) VALUES (@IdPrenotazione, @IdServizio, @DataServizio, @Quantita, @PrezzoUnitario)",
                        connection);
                    command.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);
                    command.Parameters.AddWithValue("@IdServizio", idServizio);
                    command.Parameters.AddWithValue("@DataServizio", dataServizio);
                    command.Parameters.AddWithValue("@Quantita", quantita);
                    command.Parameters.AddWithValue("@PrezzoUnitario", prezzoUnitario);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {                
                throw new ApplicationException("Errore Database", ex);
            }
        }

        // Metodo per rimuovere un servizio da una prenotazione
        public async Task<bool> RemoveServizioFromPrenotazioneAsync(int idPrenotazione, int idServizio)
        {
            try
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
            catch (SqlException ex)
            {                
                throw new ApplicationException("Database error occurred while removing the service from the reservation.", ex);
            }
        }

        // Metodo per ottenere tutti i servizi associati a una prenotazione
        public async Task<IEnumerable<Servizio>> GetServiziByPrenotazioneIdAsync(int idPrenotazione)
        {
            var servizi = new List<Servizio>();

            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand(
                        "SELECT s.* FROM Servizi s INNER JOIN ServiziPrenotazione sp ON s.IdServizio = sp.IdServizio WHERE sp.IdPrenotazione = @IdPrenotazione",
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
            }
            catch (SqlException ex)
            {
               
                throw new ApplicationException("Database error occurred while retrieving services for the reservation.", ex);
            }

            return servizi;
        }
    }
}
