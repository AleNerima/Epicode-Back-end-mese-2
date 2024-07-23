using AlbergoApp.Models;

namespace AlbergoApp.Services.Interfaces
{
    public interface IServizioService
    {
        Task<int> CreateServizioAsync(Servizio servizio);
        Task<Servizio?> GetServizioByIdAsync(int idServizio);
        Task<IEnumerable<Servizio>> GetAllServiziAsync();
        Task<bool> UpdateServizioAsync(Servizio servizio);
        Task<bool> DeleteServizioAsync(int idServizio);
    }
}
