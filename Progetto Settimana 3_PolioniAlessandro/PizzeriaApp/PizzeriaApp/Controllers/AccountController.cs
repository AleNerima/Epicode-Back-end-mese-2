using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Models;
using System.Security.Claims;


public class AccountController : Controller
{
    private readonly PizzeriaContextDb _context;

    public AccountController(PizzeriaContextDb context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string nome, string email, string password, bool isAdmin)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var utente = new Utente
        {
            Nome = nome,
            Email = email,
            PasswordHash = hashedPassword,
            IsAdmin = isAdmin
        };

        _context.Utenti.Add(utente);
        await _context.SaveChangesAsync();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var utente = await _context.Utenti.FirstOrDefaultAsync(u => u.Email == email);

        if (utente == null || !BCrypt.Net.BCrypt.Verify(password, utente.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, utente.Email),
            new Claim(ClaimTypes.Role, utente.IsAdmin ? "Admin" : "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
