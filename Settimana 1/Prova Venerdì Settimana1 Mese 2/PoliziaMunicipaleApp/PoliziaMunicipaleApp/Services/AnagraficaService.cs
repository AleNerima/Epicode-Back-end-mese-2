using System.Data.SqlClient;
using PoliziaMunicipaleApp.Models;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Services
{
    public class AnagraficaService : IAnagraficaService
    {
        private readonly IDatabaseConnectionService _databaseConnectionService;

        public AnagraficaService(IDatabaseConnectionService databaseConnectionService)
        {
            _databaseConnectionService = databaseConnectionService;
        }

        public async Task<IEnumerable<Anagrafica>> GetAllAsync()
        {
            var result = new List<Anagrafica>();

            try
            {
                using (var connection = _databaseConnectionService.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("SELECT * FROM ANAGRAFICA", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(new Anagrafica
                                {
                                    Idanagrafica = reader.GetInt32(reader.GetOrdinal("idanagrafica")),
                                    Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                    Indirizzo = reader.IsDBNull(reader.GetOrdinal("Indirizzo")) ? null : reader.GetString(reader.GetOrdinal("Indirizzo")),
                                    Citta = reader.IsDBNull(reader.GetOrdinal("Citta")) ? null : reader.GetString(reader.GetOrdinal("Citta")),
                                    CAP = reader.IsDBNull(reader.GetOrdinal("CAP")) ? null : reader.GetString(reader.GetOrdinal("CAP")),
                                    CodiceFiscale = reader.IsDBNull(reader.GetOrdinal("CodiceFiscale")) ? null : reader.GetString(reader.GetOrdinal("CodiceFiscale"))
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                
                throw new Exception("Si è verificato un errore durante il recupero dei dati.", ex);
            }

            return result;
        }

        public async Task<Anagrafica?> GetByIdAsync(int id)
        {
            Anagrafica? anagrafica = null;

            try
            {
                using (var connection = _databaseConnectionService.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("SELECT * FROM ANAGRAFICA WHERE idanagrafica = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                anagrafica = new Anagrafica
                                {
                                    Idanagrafica = reader.GetInt32(reader.GetOrdinal("idanagrafica")),
                                    Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                    Indirizzo = reader.IsDBNull(reader.GetOrdinal("Indirizzo")) ? null : reader.GetString(reader.GetOrdinal("Indirizzo")),
                                    Citta = reader.IsDBNull(reader.GetOrdinal("Citta")) ? null : reader.GetString(reader.GetOrdinal("Citta")),
                                    CAP = reader.IsDBNull(reader.GetOrdinal("CAP")) ? null : reader.GetString(reader.GetOrdinal("CAP")),
                                    CodiceFiscale = reader.IsDBNull(reader.GetOrdinal("CodiceFiscale")) ? null : reader.GetString(reader.GetOrdinal("CodiceFiscale"))
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                
                throw new Exception("Si è verificato un errore durante il recupero dei dati.", ex);
            }

            return anagrafica;
        }

        public async Task AddAsync(Anagrafica anagrafica)
        {
            try
            {
                using (var connection = _databaseConnectionService.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(
                        "INSERT INTO ANAGRAFICA (Cognome, Nome, Indirizzo, Citta, CAP, CodiceFiscale) VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @CAP, @CodiceFiscale)", connection))
                    {
                        command.Parameters.AddWithValue("@Cognome", anagrafica.Cognome ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Nome", anagrafica.Nome ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Citta", anagrafica.Citta ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CAP", anagrafica.CAP ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CodiceFiscale", anagrafica.CodiceFiscale ?? (object)DBNull.Value);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                
                throw new Exception("Si è verificato un errore durante l'inserimento dei dati.", ex);
            }
        }

        public async Task UpdateAsync(Anagrafica anagrafica)
        {
            try
            {
                using (var connection = _databaseConnectionService.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(
                        "UPDATE ANAGRAFICA SET Cognome = @Cognome, Nome = @Nome, Indirizzo = @Indirizzo, Citta = @Citta, CAP = @CAP, CodiceFiscale = @CodiceFiscale WHERE idanagrafica = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", anagrafica.Idanagrafica);
                        command.Parameters.AddWithValue("@Cognome", anagrafica.Cognome ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Nome", anagrafica.Nome ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Citta", anagrafica.Citta ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CAP", anagrafica.CAP ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CodiceFiscale", anagrafica.CodiceFiscale ?? (object)DBNull.Value);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
               
                throw new Exception("Si è verificato un errore durante l'aggiornamento dei dati.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using (var connection = _databaseConnectionService.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("DELETE FROM ANAGRAFICA WHERE idanagrafica = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                
                throw new Exception("Si è verificato un errore durante l'eliminazione dei dati.", ex);
            }
        }
    }
}
