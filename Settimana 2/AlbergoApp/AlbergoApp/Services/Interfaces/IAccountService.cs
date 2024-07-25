namespace AlbergoApp.Services.Interfaces
{
    public interface IAccountService
    {
        // Modifica il metodo di validazione per includere il ruolo come parametro out
        bool ValidateUser(string username, string password, out string role);

        // Modifica il metodo di registrazione per includere il ruolo come parametro
        void RegisterUser(string username, string password, string nome, string cognome, string role);
    }
}
