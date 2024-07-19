using System.Data.SqlClient;
using PoliziaMunicipaleApp.Models;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Services
{
    public class TipoViolazioneService : ITipoViolazioneService
    {
        private readonly IDatabaseConnectionService _databaseConnectionService;

        public TipoViolazioneService(IDatabaseConnectionService databaseConnectionService)
        {
            _databaseConnectionService = databaseConnectionService;
        }

        public async Task<IEnumerable<TipoViolazione>> GetAllAsync()
        {
            var result = new List<TipoViolazione>();

            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM TIPO_VIOLAZIONE", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TipoViolazione
                            {
                                Idviolazione = reader.GetInt32(reader.GetOrdinal("idviolazione")),
                                Descrizione = reader.IsDBNull(reader.GetOrdinal("descrizione")) ? null : reader.GetString(reader.GetOrdinal("descrizione"))
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task<TipoViolazione?> GetByIdAsync(int id)
        {
            TipoViolazione? tipoViolazione = null;

            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM TIPO_VIOLAZIONE WHERE idviolazione = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            tipoViolazione = new TipoViolazione
                            {
                                Idviolazione = reader.GetInt32(reader.GetOrdinal("idviolazione")),
                                Descrizione = reader.IsDBNull(reader.GetOrdinal("descrizione")) ? null : reader.GetString(reader.GetOrdinal("descrizione"))
                            };
                        }
                    }
                }
            }

            return tipoViolazione;
        }

        public async Task AddAsync(TipoViolazione tipoViolazione)
        {
            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "INSERT INTO TIPO_VIOLAZIONE (descrizione) VALUES (@Descrizione)", connection))
                {
                    command.Parameters.AddWithValue("@Descrizione", tipoViolazione.Descrizione ?? (object)DBNull.Value);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(TipoViolazione tipoViolazione)
        {
            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "UPDATE TIPO_VIOLAZIONE SET descrizione = @Descrizione WHERE idviolazione = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", tipoViolazione.Idviolazione);
                    command.Parameters.AddWithValue("@Descrizione", tipoViolazione.Descrizione ?? (object)DBNull.Value);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM TIPO_VIOLAZIONE WHERE idviolazione = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
