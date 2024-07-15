using DittaSpedizioniApp.Models;

using System.Data.SqlClient;

namespace DittaSpedizioniApp.Services
{
    public class ClienteService : IClienteService    
    {
        private readonly DatabaseConnection _databaseConnection;

        public ClienteService(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public List<Cliente> GetClienti()
        {
            List<Cliente> clienti = new List<Cliente>();

            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "SELECT IdCliente, Nome, Tipo, CodiceFiscale, PartitaIVA FROM Clienti";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cliente = new Cliente
                            {
                                IdCliente = (int)reader["IdCliente"],
                                Nome = reader["Nome"].ToString(),
                                Tipo = reader["Tipo"].ToString(),
                                CodiceFiscale = reader["CodiceFiscale"]?.ToString(),
                                PartitaIVA = reader["PartitaIVA"]?.ToString()
                            };

                            clienti.Add(cliente);
                        }
                    }
                }
            }

            return clienti;
        }

        public Cliente? GetClienteById(int id)
        {
            Cliente? cliente = null;

            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "SELECT IdCliente, Nome, Tipo, CodiceFiscale, PartitaIVA FROM Clienti WHERE IdCliente = @Id";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                IdCliente = (int)reader["IdCliente"],
                                Nome = reader["Nome"].ToString(),
                                Tipo = reader["Tipo"].ToString(),
                                CodiceFiscale = reader["CodiceFiscale"]?.ToString(),
                                PartitaIVA = reader["PartitaIVA"]?.ToString()
                            };
                        }
                    }
                }
            }

            return cliente;
        }

        public void AggiungiCliente(Cliente cliente)
        {
            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "INSERT INTO Clienti (Nome, Tipo, CodiceFiscale, PartitaIVA) " +
                               "VALUES (@Nome, @Tipo, @CodiceFiscale, @PartitaIVA)";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Tipo", cliente.Tipo);
                    command.Parameters.AddWithValue("@CodiceFiscale", (object?)cliente.CodiceFiscale ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PartitaIVA", (object?)cliente.PartitaIVA ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ModificaCliente(Cliente cliente)
        {
            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "UPDATE Clienti SET Nome = @Nome, Tipo = @Tipo, CodiceFiscale = @CodiceFiscale, PartitaIVA = @PartitaIVA " +
                               "WHERE IdCliente = @Id";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", cliente.IdCliente);
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Tipo", cliente.Tipo);
                    command.Parameters.AddWithValue("@CodiceFiscale", (object?)cliente.CodiceFiscale ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PartitaIVA", (object?)cliente.PartitaIVA ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminaCliente(int id)
        {
            using (var connection = _databaseConnection.GetOpenConnection())
            {
                string query = "DELETE FROM Clienti WHERE IdCliente = @Id";

                using (var command = new SqlCommand(query, (SqlConnection)connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
