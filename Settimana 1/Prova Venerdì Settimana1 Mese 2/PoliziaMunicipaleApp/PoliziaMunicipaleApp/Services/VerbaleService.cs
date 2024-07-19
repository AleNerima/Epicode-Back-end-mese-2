using System.Data.SqlClient;
using PoliziaMunicipaleApp.Models;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Services
{
    public class VerbaleService : IVerbaleService
    {
        private readonly IDatabaseConnectionService _databaseConnectionService;
        private readonly IAnagraficaService _anagraficaService;
        private readonly ITipoViolazioneService _tipoViolazioneService;

        public VerbaleService(IDatabaseConnectionService databaseConnectionService, IAnagraficaService anagraficaService, ITipoViolazioneService tipoViolazioneService)
        {
            _databaseConnectionService = databaseConnectionService;
            _anagraficaService = anagraficaService;
            _tipoViolazioneService = tipoViolazioneService;
        }

        public async Task<IEnumerable<Verbale>> GetAllAsync()
        {
            var result = new List<Verbale>();

            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM VERBALE", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var verbale = new Verbale
                            {
                                Idverbale = reader.GetInt32(reader.GetOrdinal("idverbale")),
                                DataViolazione = reader.IsDBNull(reader.GetOrdinal("DataViolazione")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                                IndirizzoViolazione = reader.IsDBNull(reader.GetOrdinal("IndirizzoViolazione")) ? null : reader.GetString(reader.GetOrdinal("IndirizzoViolazione")),
                                NominativoAgente = reader.IsDBNull(reader.GetOrdinal("NominativoAgente")) ? null : reader.GetString(reader.GetOrdinal("NominativoAgente")),
                                DataTrascrizioneVerbale = reader.IsDBNull(reader.GetOrdinal("DataTrascrizioneVerbale")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataTrascrizioneVerbale")),
                                Importo = reader.IsDBNull(reader.GetOrdinal("Importo")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Importo")),
                                DecurtamentoPunti = reader.IsDBNull(reader.GetOrdinal("DecurtamentoPunti")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti")),
                                Idanagrafica = reader.IsDBNull(reader.GetOrdinal("idanagrafica")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("idanagrafica")),
                                Idviolazione = reader.IsDBNull(reader.GetOrdinal("idviolazione")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("idviolazione"))
                            };

                            // Recupera Anagrafica e TipoViolazione separatamente
                            if (verbale.Idanagrafica.HasValue)
                            {
                                verbale.Anagrafica = await _anagraficaService.GetByIdAsync(verbale.Idanagrafica.Value);
                            }
                            if (verbale.Idviolazione.HasValue)
                            {
                                verbale.TipoViolazione = await _tipoViolazioneService.GetByIdAsync(verbale.Idviolazione.Value);
                            }

                            result.Add(verbale);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<Verbale?> GetByIdAsync(int id)
        {
            Verbale? verbale = null;

            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM VERBALE WHERE idverbale = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            verbale = new Verbale
                            {
                                Idverbale = reader.GetInt32(reader.GetOrdinal("idverbale")),
                                DataViolazione = reader.IsDBNull(reader.GetOrdinal("DataViolazione")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                                IndirizzoViolazione = reader.IsDBNull(reader.GetOrdinal("IndirizzoViolazione")) ? null : reader.GetString(reader.GetOrdinal("IndirizzoViolazione")),
                                NominativoAgente = reader.IsDBNull(reader.GetOrdinal("NominativoAgente")) ? null : reader.GetString(reader.GetOrdinal("NominativoAgente")),
                                DataTrascrizioneVerbale = reader.IsDBNull(reader.GetOrdinal("DataTrascrizioneVerbale")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DataTrascrizioneVerbale")),
                                Importo = reader.IsDBNull(reader.GetOrdinal("Importo")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Importo")),
                                DecurtamentoPunti = reader.IsDBNull(reader.GetOrdinal("DecurtamentoPunti")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti")),
                                Idanagrafica = reader.IsDBNull(reader.GetOrdinal("idanagrafica")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("idanagrafica")),
                                Idviolazione = reader.IsDBNull(reader.GetOrdinal("idviolazione")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("idviolazione"))
                            };

                            // Recupera Anagrafica e TipoViolazione separatamente
                            if (verbale.Idanagrafica.HasValue)
                            {
                                verbale.Anagrafica = await _anagraficaService.GetByIdAsync(verbale.Idanagrafica.Value);
                            }
                            if (verbale.Idviolazione.HasValue)
                            {
                                verbale.TipoViolazione = await _tipoViolazioneService.GetByIdAsync(verbale.Idviolazione.Value);
                            }
                        }
                    }
                }
            }

            return verbale;
        }

        public async Task AddAsync(Verbale verbale)
        {
            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, NominativoAgente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, idanagrafica, idviolazione) VALUES (@DataViolazione, @IndirizzoViolazione, @NominativoAgente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @Idanagrafica, @Idviolazione)", connection))
                {
                    command.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NominativoAgente", verbale.NominativoAgente ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Importo", verbale.Importo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Idanagrafica", verbale.Idanagrafica ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Idviolazione", verbale.Idviolazione ?? (object)DBNull.Value);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task DeleteAsync(int id)
        {
            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM VERBALE WHERE idverbale = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
