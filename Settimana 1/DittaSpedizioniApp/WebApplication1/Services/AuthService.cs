using Microsoft.AspNetCore.Identity;
using DittaSpedizioniApp.Models; // Assicurati di importare il namespace corretto per ApplicationUser
using System.Threading.Tasks;

namespace DittaSpedizioniApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        // Aggiungi altri metodi per la gestione dell'autenticazione, come login, logout, ecc.
    }
}

