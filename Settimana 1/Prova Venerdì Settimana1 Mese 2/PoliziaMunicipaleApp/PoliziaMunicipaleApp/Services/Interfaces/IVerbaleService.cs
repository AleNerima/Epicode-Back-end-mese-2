using PoliziaMunicipaleApp.Models;

namespace PoliziaMunicipaleApp.Services.Interfaces
{
    public interface IVerbaleService
    {
        Task<IEnumerable<Verbale>> GetAllAsync();
        Task<Verbale?> GetByIdAsync(int id);
        Task AddAsync(Verbale verbale);
        Task DeleteAsync(int id);
    }
}
