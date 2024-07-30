using PizzeriaApp.Models;


    public interface IAuthService
    {
        Task<User> RegisterAsync(string email, string password, string nome, string telefono, string role);
        Task<User> LoginAsync(string email, string password);
    }
