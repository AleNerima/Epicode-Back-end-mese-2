using AlbergoApp.Models;

namespace AlbergoApp.Services.Interfaces
{
    public interface IServizioService
    {
        // Metodi per Servizio
        Task<int> CreateServizioAsync(Servizio servizio);
        Task<Servizio?> GetServizioByIdAsync(int idServizio);
        Task<IEnumerable<Servizio>> GetAllServiziAsync();
        Task<bool> UpdateServizioAsync(Servizio servizio);
        Task<bool> DeleteServizioAsync(int idServizio);

        // Metodi per ServiziPrenotazione
        Task<IEnumerable<Servizio>> GetServiziByPrenotazioneIdAsync(int idPrenotazione);
        Task<bool> AddServizioToPrenotazioneAsync(int idPrenotazione, int idServizio, DateTime dataServizio, int quantita, decimal prezzoUnitario, decimal prezzoTotale);
        Task<bool> RemoveServizioFromPrenotazioneAsync(int idPrenotazione, int idServizio);
    }
}
