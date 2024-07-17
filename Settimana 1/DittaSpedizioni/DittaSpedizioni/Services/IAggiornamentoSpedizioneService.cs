using DittaSpedizioni.Models;


namespace DittaSpedizioni.Interfaces
{
    public interface IAggiornamentoSpedizioneService
    {
        List<AggiornamentoSpedizione> GetAggiornamentiSpedizione(int idSpedizione);
        AggiornamentoSpedizione GetAggiornamentoById(int id);
        void AddAggiornamentoSpedizione(AggiornamentoSpedizione aggiornamento);
        void UpdateAggiornamentoSpedizione(AggiornamentoSpedizione aggiornamento);
        void DeleteAggiornamentoSpedizione(int id);
    }
}
