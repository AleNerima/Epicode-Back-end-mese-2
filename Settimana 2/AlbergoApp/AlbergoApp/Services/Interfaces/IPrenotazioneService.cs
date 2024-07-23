using AlbergoApp.Models;

namespace AlbergoApp.Services.Interfaces
{
    public interface IPrenotazioneService
    {
        Task<int> CreatePrenotazioneAsync(Prenotazione prenotazione);
        Task<Prenotazione?> GetPrenotazioneByIdAsync(int idPrenotazione);
        Task<IEnumerable<Prenotazione>> GetAllPrenotazioniAsync();
        Task<bool> UpdatePrenotazioneAsync(Prenotazione prenotazione);
        Task<bool> DeletePrenotazioneAsync(int idPrenotazione);
        Task<IEnumerable<Prenotazione>> GetPrenotazioniByClienteIdAsync(int idCliente);
        Task<int> GetNumeroPrenotazioniPensioneCompletaAsync();
    }
}
