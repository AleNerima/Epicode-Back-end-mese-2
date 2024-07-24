﻿using System.Data.SqlClient;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;

namespace AlbergoApp.Services
{
    public class PrenotazioneService : IPrenotazioneService
    {
        private readonly IDatabaseService _databaseService;

        public PrenotazioneService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreatePrenotazioneAsync(Prenotazione prenotazione)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "INSERT INTO Prenotazioni (IdCliente, IdCamera, DataPrenotazione, NumeroProgressivo, Anno, PeriodoSoggiornoDal, PeriodoSoggiornoAl, CaparraConfirmatoria, Tariffa, TipoSoggiorno, Stato) " +
                    "OUTPUT INSERTED.IdPrenotazione VALUES (@IdCliente, @IdCamera, @DataPrenotazione, @NumeroProgressivo, @Anno, @PeriodoSoggiornoDal, @PeriodoSoggiornoAl, @CaparraConfirmatoria, @Tariffa, @TipoSoggiorno, @Stato)",
                    connection);
                command.Parameters.AddWithValue("@IdCliente", prenotazione.IdCliente);
                command.Parameters.AddWithValue("@IdCamera", prenotazione.IdCamera);
                command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                command.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                command.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                command.Parameters.AddWithValue("@PeriodoSoggiornoDal", prenotazione.PeriodoSoggiornoDal);
                command.Parameters.AddWithValue("@PeriodoSoggiornoAl", prenotazione.PeriodoSoggiornoAl);
                command.Parameters.AddWithValue("@CaparraConfirmatoria", prenotazione.CaparraConfirmatoria);
                command.Parameters.AddWithValue("@Tariffa", prenotazione.Tariffa);
                command.Parameters.AddWithValue("@TipoSoggiorno", prenotazione.TipoSoggiorno);
                command.Parameters.AddWithValue("@Stato", prenotazione.Stato);

                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
        }

        public async Task<Prenotazione?> GetPrenotazioneByIdAsync(int idPrenotazione)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE IdPrenotazione = @IdPrenotazione", connection);
                command.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Prenotazione
                        {
                            IdPrenotazione = (int)reader["IdPrenotazione"],
                            IdCliente = (int)reader["IdCliente"],
                            IdCamera = (int)reader["IdCamera"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivo = (int)reader["NumeroProgressivo"],
                            Anno = (int)reader["Anno"],
                            PeriodoSoggiornoDal = (DateTime)reader["PeriodoSoggiornoDal"],
                            PeriodoSoggiornoAl = (DateTime)reader["PeriodoSoggiornoAl"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            TipoSoggiorno = reader["TipoSoggiorno"].ToString(),
                            Stato = reader["Stato"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Prenotazione>> GetAllPrenotazioniAsync()
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Prenotazioni", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        prenotazioni.Add(new Prenotazione
                        {
                            IdPrenotazione = (int)reader["IdPrenotazione"],
                            IdCliente = (int)reader["IdCliente"],
                            IdCamera = (int)reader["IdCamera"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivo = (int)reader["NumeroProgressivo"],
                            Anno = (int)reader["Anno"],
                            PeriodoSoggiornoDal = (DateTime)reader["PeriodoSoggiornoDal"],
                            PeriodoSoggiornoAl = (DateTime)reader["PeriodoSoggiornoAl"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            TipoSoggiorno = reader["TipoSoggiorno"].ToString(),
                            Stato = reader["Stato"].ToString()
                        });
                    }
                }
            }
            return prenotazioni;
        }

        public async Task<bool> UpdatePrenotazioneAsync(Prenotazione prenotazione)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE Prenotazioni SET IdCliente = @IdCliente, IdCamera = @IdCamera, DataPrenotazione = @DataPrenotazione, NumeroProgressivo = @NumeroProgressivo, Anno = @Anno, PeriodoSoggiornoDal = @PeriodoSoggiornoDal, PeriodoSoggiornoAl = @PeriodoSoggiornoAl, CaparraConfirmatoria = @CaparraConfirmatoria, Tariffa = @Tariffa, TipoSoggiorno = @TipoSoggiorno, Stato = @Stato WHERE IdPrenotazione = @IdPrenotazione",
                    connection);
                command.Parameters.AddWithValue("@IdCliente", prenotazione.IdCliente);
                command.Parameters.AddWithValue("@IdCamera", prenotazione.IdCamera);
                command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                command.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                command.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                command.Parameters.AddWithValue("@PeriodoSoggiornoDal", prenotazione.PeriodoSoggiornoDal);
                command.Parameters.AddWithValue("@PeriodoSoggiornoAl", prenotazione.PeriodoSoggiornoAl);
                command.Parameters.AddWithValue("@CaparraConfirmatoria", prenotazione.CaparraConfirmatoria);
                command.Parameters.AddWithValue("@Tariffa", prenotazione.Tariffa);
                command.Parameters.AddWithValue("@TipoSoggiorno", prenotazione.TipoSoggiorno);
                command.Parameters.AddWithValue("@Stato", prenotazione.Stato);
                command.Parameters.AddWithValue("@IdPrenotazione", prenotazione.IdPrenotazione);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeletePrenotazioneAsync(int idPrenotazione)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Prenotazioni WHERE IdPrenotazione = @IdPrenotazione", connection);
                command.Parameters.AddWithValue("@IdPrenotazione", idPrenotazione);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Prenotazione>> GetPrenotazioniByClienteIdAsync(int idCliente)
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE IdCliente = @IdCliente", connection);
                command.Parameters.AddWithValue("@IdCliente", idCliente);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        prenotazioni.Add(new Prenotazione
                        {
                            IdPrenotazione = (int)reader["IdPrenotazione"],
                            IdCliente = (int)reader["IdCliente"],
                            IdCamera = (int)reader["IdCamera"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            NumeroProgressivo = (int)reader["NumeroProgressivo"],
                            Anno = (int)reader["Anno"],
                            PeriodoSoggiornoDal = (DateTime)reader["PeriodoSoggiornoDal"],
                            PeriodoSoggiornoAl = (DateTime)reader["PeriodoSoggiornoAl"],
                            CaparraConfirmatoria = (decimal)reader["CaparraConfirmatoria"],
                            Tariffa = (decimal)reader["Tariffa"],
                            TipoSoggiorno = reader["TipoSoggiorno"].ToString(),
                            Stato = reader["Stato"].ToString()
                        });
                    }
                }
            }
            return prenotazioni;
        }

        public async Task<int> GetNumeroPrenotazioniPensioneCompletaAsync()
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT COUNT(*) FROM Prenotazioni WHERE TipoSoggiorno = 'Pensione Completa'", connection);

                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
        }
    }
}