using DittaSpedizioni.Models;


namespace DittaSpedizioni.Interfaces
{
    public interface ISpedizioneService
    {
        List<Spedizione> GetSpedizioni();
        Spedizione GetSpedizioneById(int id);
        void AddSpedizione(Spedizione spedizione);
        void UpdateSpedizione(Spedizione spedizione);
        void DeleteSpedizione(int id);
    }
}
