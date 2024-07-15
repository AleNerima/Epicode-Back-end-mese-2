using DittaSpedizioniApp.Models;


namespace DittaSpedizioniApp.Services
{
    public interface IAggiornamentoSpedizioneService
    {
        List<AggiornamentoSpedizione> GetAggiornamentiBySpedizioneId(int id);
        void AggiungiAggiornamento(AggiornamentoSpedizione aggiornamento);
    }
}
