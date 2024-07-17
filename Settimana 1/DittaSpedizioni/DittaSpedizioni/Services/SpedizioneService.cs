using System.Data.SqlClient;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;


namespace DittaSpedizioni.Services
{
    public class SpedizioneService : ISpedizioneService
    {
        private readonly DatabaseConnection _databaseConnection;

        public SpedizioneService(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<Spedizione> GetSpedizioni()
        {
            var spedizioni = new List<Spedizione>();

            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Spedizioni";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    spedizioni.Add(new Spedizione
                    {
                        IdSpedizione = (int)reader["IdSpedizione"],
                        Cliente = (int)reader["Cliente"],
                        NumeroIdentificativo = reader["NumeroIdentificativo"].ToString(),
                        DataSpedizione = (DateTime)reader["DataSpedizione"],
                        Peso = (decimal)reader["Peso"],
                        CittaDestinataria = reader["CittaDestinataria"].ToString(),
                        IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        Costo = (decimal)reader["Costo"],
                        DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"]
                    });
                }
            }

            return spedizioni;
        }

        public Spedizione GetSpedizioneById(int id)
        {
            Spedizione spedizione = null;

            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Spedizioni WHERE IdSpedizione = @IdSpedizione";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdSpedizione", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    spedizione = new Spedizione
                    {
                        IdSpedizione = (int)reader["IdSpedizione"],
                        Cliente = (int)reader["Cliente"],
                        NumeroIdentificativo = reader["NumeroIdentificativo"].ToString(),
                        DataSpedizione = (DateTime)reader["DataSpedizione"],
                        Peso = (decimal)reader["Peso"],
                        CittaDestinataria = reader["CittaDestinataria"].ToString(),
                        IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                        NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                        Costo = (decimal)reader["Costo"],
                        DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"]
                    };
                }
            }

            return spedizione;
        }

        public void AddSpedizione(Spedizione spedizione)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "INSERT INTO Spedizioni (Cliente, NumeroIdentificativo, DataSpedizione, Peso, CittaDestinataria, IndirizzoDestinatario, NominativoDestinatario, Costo, DataConsegnaPrevista) VALUES (@Cliente, @NumeroIdentificativo, @DataSpedizione, @Peso, @CittaDestinataria, @IndirizzoDestinatario, @NominativoDestinatario, @Costo, @DataConsegnaPrevista)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cliente", spedizione.Cliente);
                cmd.Parameters.AddWithValue("@NumeroIdentificativo", spedizione.NumeroIdentificativo);
                cmd.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                cmd.Parameters.AddWithValue("@Peso", spedizione.Peso);
                cmd.Parameters.AddWithValue("@CittaDestinataria", spedizione.CittaDestinataria);
                cmd.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                cmd.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                cmd.Parameters.AddWithValue("@Costo", spedizione.Costo);
                cmd.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSpedizione(Spedizione spedizione)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "UPDATE Spedizioni SET Cliente = @Cliente, NumeroIdentificativo = @NumeroIdentificativo, DataSpedizione = @DataSpedizione, Peso = @Peso, CittaDestinataria = @CittaDestinataria, IndirizzoDestinatario = @IndirizzoDestinatario, NominativoDestinatario = @NominativoDestinatario, Costo = @Costo, DataConsegnaPrevista = @DataConsegnaPrevista WHERE IdSpedizione = @IdSpedizione";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cliente", spedizione.Cliente);
                cmd.Parameters.AddWithValue("@NumeroIdentificativo", spedizione.NumeroIdentificativo);
                cmd.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                cmd.Parameters.AddWithValue("@Peso", spedizione.Peso);
                cmd.Parameters.AddWithValue("@CittaDestinataria", spedizione.CittaDestinataria);
                cmd.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                cmd.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                cmd.Parameters.AddWithValue("@Costo", spedizione.Costo);
                cmd.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);
                cmd.Parameters.AddWithValue("@IdSpedizione", spedizione.IdSpedizione);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSpedizione(int id)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "DELETE FROM Spedizioni WHERE IdSpedizione = @IdSpedizione";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdSpedizione", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
