﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

        // Metodo per creare un nuovo servizio
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
                // Log error
                throw new ApplicationException("Database error occurred while creating the service.", ex);
            }
        }

        // Metodo per ottenere un servizio per ID
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
                // Log error
                throw new ApplicationException("Database error occurred while retrieving the service.", ex);
            }
            return null;
        }

        // Metodo per ottenere tutti i servizi
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
                // Log error
                throw new ApplicationException("Database error occurred while retrieving all services.", ex);
            }
            return servizi;
        }

        // Metodo per aggiornare un servizio
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
                // Log error
                throw new ApplicationException("Database error occurred while updating the service.", ex);
            }
        }

        // Metodo per eliminare un servizio
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
                // Log error
                throw new ApplicationException("Database error occurred while deleting the service.", ex);
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
            }
            catch (SqlException ex)
            {
                // Log error
                throw new ApplicationException("Database error occurred while retrieving services for the reservation.", ex);
            }
            return servizi;
        }

        // Metodo per aggiungere un servizio a una prenotazione
        public async Task<bool> AddServizioToPrenotazioneAsync(int idPrenotazione, int idServizio, DateTime dataServizio, int quantita, decimal prezzoUnitario, decimal prezzoTotale)
        {
            try
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
            catch (SqlException ex)
            {
                // Log error
                throw new ApplicationException("Database error occurred while adding the service to the reservation.", ex);
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
                // Log error
                throw new ApplicationException("Database error occurred while removing the service from the reservation.", ex);
            }
        }
    }
}
