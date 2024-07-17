using DittaSpedizioni.Models;

namespace DittaSpedizioni.Interfaces
{
    public interface IUtenteService
    {
        Utente GetUtenteByEmail(string email);
        void AddUtente(Utente utente);
    }
}

