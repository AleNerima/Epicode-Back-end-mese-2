using System.Data.SqlClient;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;

namespace AlbergoApp.Services
{
    public class CameraService : ICameraService
    {
        private readonly IDatabaseService _databaseService;

        public CameraService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreateCameraAsync(Camera camera)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Camere (Numero, Descrizione, Tipologia) OUTPUT INSERTED.IdCamera VALUES (@Numero, @Descrizione, @Tipologia)", connection);
                command.Parameters.AddWithValue("@Numero", camera.Numero);
                command.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
                command.Parameters.AddWithValue("@Tipologia", camera.Tipologia);

                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
        }

        public async Task<Camera?> GetCameraByIdAsync(int idCamera)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Camere WHERE IdCamera = @IdCamera", connection);
                command.Parameters.AddWithValue("@IdCamera", idCamera);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Camera
                        {
                            IdCamera = (int)reader["IdCamera"],
                            Numero = (int)reader["Numero"],
                            Descrizione = reader["Descrizione"].ToString(),
                            Tipologia = reader["Tipologia"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<Camera>> GetAllCamereAsync()
        {
            var camere = new List<Camera>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Camere", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        camere.Add(new Camera
                        {
                            IdCamera = (int)reader["IdCamera"],
                            Numero = (int)reader["Numero"],
                            Descrizione = reader["Descrizione"].ToString(),
                            Tipologia = reader["Tipologia"].ToString()
                        });
                    }
                }
            }
            return camere;
        }

        public async Task<bool> UpdateCameraAsync(Camera camera)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Camere SET Numero = @Numero, Descrizione = @Descrizione, Tipologia = @Tipologia WHERE IdCamera = @IdCamera", connection);
                command.Parameters.AddWithValue("@Numero", camera.Numero);
                command.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
                command.Parameters.AddWithValue("@Tipologia", camera.Tipologia);
                command.Parameters.AddWithValue("@IdCamera", camera.IdCamera);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteCameraAsync(int idCamera)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Camere WHERE IdCamera = @IdCamera", connection);
                command.Parameters.AddWithValue("@IdCamera", idCamera);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<Camera?> GetCameraByNumeroAsync(int numero)
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Camere WHERE Numero = @Numero", connection);
                command.Parameters.AddWithValue("@Numero", numero);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Camera
                        {
                            IdCamera = (int)reader["IdCamera"],
                            Numero = (int)reader["Numero"],
                            Descrizione = reader["Descrizione"].ToString(),
                            Tipologia = reader["Tipologia"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}
