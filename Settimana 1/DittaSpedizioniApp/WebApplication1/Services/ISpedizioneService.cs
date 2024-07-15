using DittaSpedizioniApp.Models;

namespace DittaSpedizioniApp.Services
{
    public interface ISpedizioneService
    {
        List<Spedizione> GetSpedizioni();
        Spedizione GetSpedizioneById(int id);
        void AggiungiSpedizione(Spedizione spedizione);
        void ModificaSpedizione(Spedizione spedizione);
        void EliminaSpedizione(int id);
    }
}
