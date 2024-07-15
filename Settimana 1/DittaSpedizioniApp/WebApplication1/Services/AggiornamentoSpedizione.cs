using DittaSpedizioniApp.Models;
using System.Data.SqlClient;

namespace DittaSpedizioniApp.Services
{
    public class AggiornamentoSpedizioneService : IAggiornamentoSpedizioneService
    {
        private readonly DatabaseConnection _databaseConnection;

        public AggiornamentoSpedizioneService(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<AggiornamentoSpedizione> GetAggiornamentiBySpedizioneId(int spedizioneId)
        {
            List<AggiornamentoSpedizione> aggiornamenti = new List<AggiornamentoSpedizione>();

            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "SELECT IdAggiornamento, Spedizione, Stato, Luogo, Descrizione, DataAggiornamento " +
                               "FROM AggiornamentiSpedizioni WHERE Spedizione = @SpedizioneId " +
                               "ORDER BY DataAggiornamento DESC";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@SpedizioneId", spedizioneId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var aggiornamento = new AggiornamentoSpedizione
                            {
                                IdAggiornamento = (int)reader["IdAggiornamento"],
                                Spedizione = (int)reader["Spedizione"],
                                Stato = reader["Stato"].ToString(),
                                Luogo = reader["Luogo"].ToString(),
                                Descrizione = reader["Descrizione"]?.ToString(),
                                DataAggiornamento = (DateTime)reader["DataAggiornamento"]
                            };

                            aggiornamenti.Add(aggiornamento);
                        }
                    }
                }
            }

            return aggiornamenti;
        }

        public void AggiungiAggiornamento(AggiornamentoSpedizione aggiornamento)
        {
            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "INSERT INTO AggiornamentiSpedizioni (Spedizione, Stato, Luogo, Descrizione, DataAggiornamento) " +
                               "VALUES (@Spedizione, @Stato, @Luogo, @Descrizione, @DataAggiornamento)";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Spedizione", aggiornamento.Spedizione);
                    command.Parameters.AddWithValue("@Stato", aggiornamento.Stato);
                    command.Parameters.AddWithValue("@Luogo", aggiornamento.Luogo);
                    command.Parameters.AddWithValue("@Descrizione", (object?)aggiornamento.Descrizione ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DataAggiornamento", aggiornamento.DataAggiornamento);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
