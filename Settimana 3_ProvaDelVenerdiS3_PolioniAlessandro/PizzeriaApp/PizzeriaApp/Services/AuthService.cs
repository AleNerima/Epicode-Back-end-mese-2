using PizzeriaApp.Data;
using PizzeriaApp.Models;
using Microsoft.EntityFrameworkCore;


public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Registra un nuovo utente
    public async Task<User> RegisterAsync(string email, string password, string nome, string telefono, string role)
    {
        // Crea un nuovo oggetto User con i dettagli forniti
        var user = new User
        {
            Email = email,
            Nome = nome,
            Telefono = telefono,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };

        // Aggiunge l'utente al contesto del database
        _context.Users.Add(user);
        await _context.SaveChangesAsync(); // Salva l'utente e generare un ID

        return user;
    }

    // Effettua il login di un utente esistente
    public async Task<User> LoginAsync(string email, string password)
    {
        // Cerca un utente con l'email fornita
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) // Verifica se l'utente esiste e se la password fornita è corretta (Uso BCrypt)
        {
            return user;
        }
        return null;
    }
}
