using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;
using System.Security.Claims;


namespace DittaSpedizioni.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUtenteService _utenteService;

        public AccountController(IUtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var utente = _utenteService.GetUtenteByEmail(email);
                if (utente != null && BCrypt.Net.BCrypt.Verify(password, utente.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, utente.Email),
                        new Claim(ClaimTypes.Role, utente.Ruolo)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (utente.Ruolo == "Lavoratore")
                    {
                        return RedirectToAction("Index", "Cliente");
                    }
                    else if (utente.Ruolo == "Cliente")
                    {
                        return RedirectToAction("Index", "ClienteHome");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrazioneUtente model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _utenteService.GetUtenteByEmail(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email already registered.");
                    return View("Register", model);
                }

                var newUser = new Utente
                {
                    Nome = model.Nome,
                    Cognome = model.Cognome,
                    Email = model.Email,
                    Ruolo = model.Ruolo
                };

                newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                _utenteService.AddUtente(newUser);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newUser.Email),
                    new Claim(ClaimTypes.Role, newUser.Ruolo)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Login", "Account");
            }

            return View("Register", model);
        }
    }
}
