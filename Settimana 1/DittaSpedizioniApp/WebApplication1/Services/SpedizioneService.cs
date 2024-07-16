using DittaSpedizioniApp.Models;
using System.Data.SqlClient;

namespace DittaSpedizioniApp.Services
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
            List<Spedizione> spedizioni = new List<Spedizione>();

            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "SELECT IdSpedizione, Cliente, NumeroIdentificativo, DataSpedizione, Peso, CittaDestinataria, " +
                               "IndirizzoDestinatario, NominativoDestinatario, Costo, DataConsegnaPrevista " +
                               "FROM Spedizioni";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var spedizione = new Spedizione
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

                            spedizioni.Add(spedizione);
                        }
                    }
                }
            }

            return spedizioni;
        }

        public Spedizione GetSpedizioneById(int id)
        {
            Spedizione? spedizione = null;

            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "SELECT IdSpedizione, Cliente, NumeroIdentificativo, DataSpedizione, Peso, CittaDestinataria, " +
                               "IndirizzoDestinatario, NominativoDestinatario, Costo, DataConsegnaPrevista " +
                               "FROM Spedizioni WHERE IdSpedizione = @Id";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
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
                }
            }

            return spedizione;
        }

        public void AggiungiSpedizione(Spedizione spedizione)
        {
            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "INSERT INTO Spedizioni (Cliente, NumeroIdentificativo, DataSpedizione, Peso, CittaDestinataria, " +
                               "IndirizzoDestinatario, NominativoDestinatario, Costo, DataConsegnaPrevista) " +
                               "VALUES (@Cliente, @NumeroIdentificativo, @DataSpedizione, @Peso, @CittaDestinataria, " +
                               "@IndirizzoDestinatario, @NominativoDestinatario, @Costo, @DataConsegnaPrevista)";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Cliente", spedizione.Cliente);
                    command.Parameters.AddWithValue("@NumeroIdentificativo", spedizione.NumeroIdentificativo);
                    command.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                    command.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    command.Parameters.AddWithValue("@CittaDestinataria", spedizione.CittaDestinataria);
                    command.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                    command.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                    command.Parameters.AddWithValue("@Costo", spedizione.Costo);
                    command.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ModificaSpedizione(Spedizione spedizione)
        {
            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "UPDATE Spedizioni SET Cliente = @Cliente, NumeroIdentificativo = @NumeroIdentificativo, " +
                               "DataSpedizione = @DataSpedizione, Peso = @Peso, CittaDestinataria = @CittaDestinataria, " +
                               "IndirizzoDestinatario = @IndirizzoDestinatario, NominativoDestinatario = @NominativoDestinatario, " +
                               "Costo = @Costo, DataConsegnaPrevista = @DataConsegnaPrevista " +
                               "WHERE IdSpedizione = @Id";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", spedizione.IdSpedizione);
                    command.Parameters.AddWithValue("@Cliente", spedizione.Cliente);
                    command.Parameters.AddWithValue("@NumeroIdentificativo", spedizione.NumeroIdentificativo);
                    command.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                    command.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    command.Parameters.AddWithValue("@CittaDestinataria", spedizione.CittaDestinataria);
                    command.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                    command.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                    command.Parameters.AddWithValue("@Costo", spedizione.Costo);
                    command.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminaSpedizione(int id)
        {
            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "DELETE FROM Spedizioni WHERE IdSpedizione = @Id";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
