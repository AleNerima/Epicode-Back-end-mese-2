using DittaSpedizioniApp.Models; // Assicurati di importare il namespace corretto per ApplicationUser
using System.Threading.Tasks;

namespace DittaSpedizioniApp.Services
{
    public interface IAuthService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        // Aggiungi altri metodi necessari per la gestione dell'autenticazione
    }
}
