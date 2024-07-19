using PoliziaMunicipaleApp.Models;

namespace PoliziaMunicipaleApp.Services.Interfaces
{
    public interface IVisualizzazioneService
    {
        Task<IEnumerable<TrasgressoreTotale>> GetTotaleVerbaliPerTrasgressoreAsync();
        Task<IEnumerable<TrasgressorePuntiTotali>> GetTotalePuntiDecurtatiPerTrasgressoreAsync();
        Task<IEnumerable<Violazione>> GetViolazioniConPuntiMaggioreDi10Async();
        Task<IEnumerable<Violazione>> GetViolazioniConImportoMaggioreDi400Async();
    }  
}
