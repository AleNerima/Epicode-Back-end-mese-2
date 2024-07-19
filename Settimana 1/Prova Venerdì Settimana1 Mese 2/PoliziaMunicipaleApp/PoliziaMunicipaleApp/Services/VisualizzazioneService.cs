using System.Data.SqlClient;
using PoliziaMunicipaleApp.Models;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Services
{
    public class VisualizzazioneService : IVisualizzazioneService
    {
        private readonly IDatabaseConnectionService _databaseConnectionService;

        public VisualizzazioneService(IDatabaseConnectionService databaseConnectionService)
        {
            _databaseConnectionService = databaseConnectionService;
        }

        public async Task<IEnumerable<TrasgressoreTotale>> GetTotaleVerbaliPerTrasgressoreAsync()
        {
            var result = new List<TrasgressoreTotale>();

            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(@"
                    SELECT a.idanagrafica, a.cognome, a.nome, COUNT(v.idverbale) AS TotaleVerbali
                    FROM anagrafica a
                    LEFT JOIN verbale v ON a.idanagrafica = v.idanagrafica
                    GROUP BY a.idanagrafica, a.cognome, a.nome", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TrasgressoreTotale
                            {
                                Idanagrafica = reader.GetInt32(reader.GetOrdinal("idanagrafica")),
                                Cognome = reader.GetString(reader.GetOrdinal("cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("nome")),
                                TotaleVerbali = reader.GetInt32(reader.GetOrdinal("TotaleVerbali"))
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<TrasgressorePuntiTotali>> GetTotalePuntiDecurtatiPerTrasgressoreAsync()
        {
            var result = new List<TrasgressorePuntiTotali>();

            try
            {
                using (var connection = _databaseConnectionService.CreateConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(@"
                SELECT a.idanagrafica, a.cognome, a.nome, COALESCE(SUM(v.decurtamentoPunti), 0) AS TotalePunti
                FROM anagrafica a
                LEFT JOIN verbale v ON a.idanagrafica = v.idanagrafica
                GROUP BY a.idanagrafica, a.cognome, a.nome", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(new TrasgressorePuntiTotali
                                {
                                    Idanagrafica = reader.GetInt32(reader.GetOrdinal("idanagrafica")),
                                    Cognome = reader.GetString(reader.GetOrdinal("cognome")),
                                    Nome = reader.GetString(reader.GetOrdinal("nome")),
                                    TotalePunti = reader.GetInt32(reader.GetOrdinal("TotalePunti"))
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                
                Console.WriteLine($"Database error: {ex.Message}");
                
                throw;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Unexpected error: {ex.Message}");
                
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<Violazione>> GetViolazioniConPuntiMaggioreDi10Async()
        {
            var result = new List<Violazione>();

            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(@"
                    SELECT v.idverbale, v.importo, a.cognome, a.nome, v.dataViolazione, v.decurtamentoPunti
                    FROM verbale v
                    JOIN anagrafica a ON v.idanagrafica = a.idanagrafica
                    WHERE v.decurtamentoPunti > 10", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Violazione
                            {
                                Idverbale = reader.GetInt32(reader.GetOrdinal("idverbale")),
                                Importo = reader.GetDecimal(reader.GetOrdinal("importo")),
                                Cognome = reader.GetString(reader.GetOrdinal("cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("nome")),
                                DataViolazione = reader.GetDateTime(reader.GetOrdinal("dataViolazione")),
                                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("decurtamentoPunti"))
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<Violazione>> GetViolazioniConImportoMaggioreDi400Async()
        {
            var result = new List<Violazione>();

            using (var connection = _databaseConnectionService.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(@"
                    SELECT v.idverbale, v.importo, a.cognome, a.nome, v.dataViolazione, v.decurtamentoPunti
                    FROM verbale v
                    JOIN anagrafica a ON v.idanagrafica = a.idanagrafica
                    WHERE v.importo > 400", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Violazione
                            {
                                Idverbale = reader.GetInt32(reader.GetOrdinal("idverbale")),
                                Importo = reader.GetDecimal(reader.GetOrdinal("importo")),
                                Cognome = reader.GetString(reader.GetOrdinal("cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("nome")),
                                DataViolazione = reader.GetDateTime(reader.GetOrdinal("dataViolazione")),
                                DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("decurtamentoPunti"))
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
