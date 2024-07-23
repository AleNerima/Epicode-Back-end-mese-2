namespace AlbergoApp.Services.Interfaces
{
    public interface IAccountService
    {
        bool ValidateUser(string username, string password);
        void RegisterUser(string username, string password, string nome, string cognome);
    }
}
