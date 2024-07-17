using System.Data.SqlClient;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;


namespace DittaSpedizioni.Services
{
    public class AggiornamentoSpedizioneService : IAggiornamentoSpedizioneService
    {
        private readonly DatabaseConnection _databaseConnection;

        public AggiornamentoSpedizioneService(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<AggiornamentoSpedizione> GetAggiornamentiSpedizione(int idSpedizione)
        {
            var aggiornamenti = new List<AggiornamentoSpedizione>();

            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "SELECT * FROM AggiornamentiSpedizioni WHERE Spedizione = @IdSpedizione";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdSpedizione", idSpedizione);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    aggiornamenti.Add(new AggiornamentoSpedizione
                    {
                        IdAggiornamento = (int)reader["IdAggiornamento"],
                        Spedizione = (int)reader["Spedizione"],
                        Stato = reader["Stato"].ToString(),
                        Luogo = reader["Luogo"].ToString(),
                        Descrizione = reader["Descrizione"].ToString(),
                        DataAggiornamento = (DateTime)reader["DataAggiornamento"],
                        Operatore = (int)reader["Operatore"]
                    });
                }
            }

            return aggiornamenti;
        }

        public AggiornamentoSpedizione GetAggiornamentoById(int id)
        {
            AggiornamentoSpedizione aggiornamento = null;

            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "SELECT * FROM AggiornamentiSpedizioni WHERE IdAggiornamento = @IdAggiornamento";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdAggiornamento", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    aggiornamento = new AggiornamentoSpedizione
                    {
                        IdAggiornamento = (int)reader["IdAggiornamento"],
                        Spedizione = (int)reader["Spedizione"],
                        Stato = reader["Stato"].ToString(),
                        Luogo = reader["Luogo"].ToString(),
                        Descrizione = reader["Descrizione"].ToString(),
                        DataAggiornamento = (DateTime)reader["DataAggiornamento"],
                        Operatore = (int)reader["Operatore"]
                    };
                }
            }

            return aggiornamento;
        }

        public void AddAggiornamentoSpedizione(AggiornamentoSpedizione aggiornamento)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "INSERT INTO AggiornamentiSpedizioni (Spedizione, Stato, Luogo, Descrizione, DataAggiornamento, Operatore) VALUES (@Spedizione, @Stato, @Luogo, @Descrizione, @DataAggiornamento, @Operatore)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Spedizione", aggiornamento.Spedizione);
                cmd.Parameters.AddWithValue("@Stato", aggiornamento.Stato);
                cmd.Parameters.AddWithValue("@Luogo", aggiornamento.Luogo);
                cmd.Parameters.AddWithValue("@Descrizione", aggiornamento.Descrizione);
                cmd.Parameters.AddWithValue("@DataAggiornamento", aggiornamento.DataAggiornamento);
                cmd.Parameters.AddWithValue("@Operatore", aggiornamento.Operatore);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateAggiornamentoSpedizione(AggiornamentoSpedizione aggiornamento)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "UPDATE AggiornamentiSpedizioni SET Spedizione = @Spedizione, Stato = @Stato, Luogo = @Luogo, Descrizione = @Descrizione, DataAggiornamento = @DataAggiornamento, Operatore = @Operatore WHERE IdAggiornamento = @IdAggiornamento";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Spedizione", aggiornamento.Spedizione);
                cmd.Parameters.AddWithValue("@Stato", aggiornamento.Stato);
                cmd.Parameters.AddWithValue("@Luogo", aggiornamento.Luogo);
                cmd.Parameters.AddWithValue("@Descrizione", aggiornamento.Descrizione);
                cmd.Parameters.AddWithValue("@DataAggiornamento", aggiornamento.DataAggiornamento);
                cmd.Parameters.AddWithValue("@Operatore", aggiornamento.Operatore);
                cmd.Parameters.AddWithValue("@IdAggiornamento", aggiornamento.IdAggiornamento);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteAggiornamentoSpedizione(int id)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "DELETE FROM AggiornamentiSpedizioni WHERE IdAggiornamento = @IdAggiornamento";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdAggiornamento", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
