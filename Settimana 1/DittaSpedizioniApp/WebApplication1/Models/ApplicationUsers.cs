using Microsoft.AspNetCore.Identity;

namespace DittaSpedizioniApp.Models
{
    // Estende IdentityUser per ottenere funzionalità di autenticazione predefinite
    public class ApplicationUser : IdentityUser
    {
        // Aggiungi qui eventuali proprietà personalizzate
        public string Nome { get; set; }
        public string Cognome { get; set; }
    }
}
