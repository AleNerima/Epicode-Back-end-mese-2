using System.Data.SqlClient;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;


namespace DittaSpedizioni.Services
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
            var clienti = new List<Cliente>();

            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Clienti";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    clienti.Add(new Cliente
                    {
                        IdCliente = (int)reader["IdCliente"],
                        Nome = reader["Nome"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        CodiceFiscale = reader["CodiceFiscale"]?.ToString(),
                        PartitaIVA = reader["PartitaIVA"]?.ToString()
                    });
                }
            }

            return clienti;
        }

        public Cliente GetClienteById(int id)
        {
            Cliente cliente = null;

            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Clienti WHERE IdCliente = @IdCliente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdCliente", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

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

            return cliente;
        }

        public void AddCliente(Cliente cliente)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "INSERT INTO Clienti (Nome, Tipo, CodiceFiscale, PartitaIVA) VALUES (@Nome, @Tipo, @CodiceFiscale, @PartitaIVA)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Tipo", cliente.Tipo);
                cmd.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PartitaIVA", cliente.PartitaIVA ?? (object)DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCliente(Cliente cliente)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "UPDATE Clienti SET Nome = @Nome, Tipo = @Tipo, CodiceFiscale = @CodiceFiscale, PartitaIVA = @PartitaIVA WHERE IdCliente = @IdCliente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Tipo", cliente.Tipo);
                cmd.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PartitaIVA", cliente.PartitaIVA ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCliente(int id)
        {
            using (var conn = _databaseConnection.GetConnection())
            {
                string query = "DELETE FROM Clienti WHERE IdCliente = @IdCliente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdCliente", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
