using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;

namespace AlbergoApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_accountService.ValidateUser(model.Username, model.Password))
                {
                    // Configura il cookie di autenticazione e reindirizza l'utente
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Username o password non validi.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _accountService.RegisterUser(model.Username, model.Password, model.Nome, model.Cognome);
                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}
