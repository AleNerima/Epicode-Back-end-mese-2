using PoliziaMunicipaleApp.Models;

namespace PoliziaMunicipaleApp.Services.Interfaces
{
    public interface ITipoViolazioneService
    {
        Task<IEnumerable<TipoViolazione>> GetAllAsync();
        Task<TipoViolazione?> GetByIdAsync(int id);
        Task AddAsync(TipoViolazione tipoViolazione);
        Task UpdateAsync(TipoViolazione tipoViolazione);
        Task DeleteAsync(int id);
    }
}
