using AlbergoApp.Models;

namespace AlbergoApp.Services.Interfaces
{
    public interface IDipendenteService
    {
        Task<int> CreateDipendenteAsync(Dipendente dipendente);
        Task<Dipendente?> GetDipendenteByIdAsync(int idDipendente);
        Task<IEnumerable<Dipendente>> GetAllDipendentiAsync();
        Task<bool> UpdateDipendenteAsync(Dipendente dipendente);
        Task<bool> DeleteDipendenteAsync(int idDipendente);
        Task<Dipendente?> GetDipendenteByUsernameAsync(string username);
    }
}
