using PoliziaMunicipaleApp.Models;

namespace PoliziaMunicipaleApp.Services.Interfaces
{
    public interface IAnagraficaService
    {
        Task<IEnumerable<Anagrafica>> GetAllAsync();
        Task<Anagrafica?> GetByIdAsync(int id);
        Task AddAsync(Anagrafica anagrafica);
        Task UpdateAsync(Anagrafica anagrafica);
        Task DeleteAsync(int id);
    }
}
