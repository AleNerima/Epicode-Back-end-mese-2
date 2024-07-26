using AlbergoApp.Models;

namespace AlbergoApp.Services.Interfaces
{
    public interface ICameraService
    {
        Task<int> CreateCameraAsync(Camera camera);
        Task<Camera?> GetCameraByIdAsync(int idCamera);
        Task<IEnumerable<Camera>> GetAllCamereAsync();
        Task<bool> UpdateCameraAsync(Camera camera);
        Task<bool> DeleteCameraAsync(int idCamera);
        Task<Camera?> GetCameraByNumeroAsync(int numero);
        Task<IEnumerable<Camera>> GetAvailableCamereAsync(DateTime startDate, DateTime endDate);
    }
}
