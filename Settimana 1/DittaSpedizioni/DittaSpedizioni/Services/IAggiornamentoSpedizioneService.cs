using DittaSpedizioni.Models;
using System.Collections.Generic;

namespace DittaSpedizioni.Interfaces
{
    public interface IAggiornamentoSpedizioneService
    {
        List<AggiornamentoSpedizione> GetAggiornamentiSpedizione(int idSpedizione);
        AggiornamentoSpedizione GetAggiornamentoById(int id);
        void AddAggiornamentoSpedizione(AggiornamentoSpedizione aggiornamento);
        void UpdateAggiornamentoSpedizione(AggiornamentoSpedizione aggiornamento);
        void DeleteAggiornamentoSpedizione(int id);
        List<AggiornamentoSpedizione> GetAggiornamentiBySpedizioneId(int idSpedizione);

        List<AggiornamentoSpedizione> GetAllAggiornamentiSpedizione();
    }
}
