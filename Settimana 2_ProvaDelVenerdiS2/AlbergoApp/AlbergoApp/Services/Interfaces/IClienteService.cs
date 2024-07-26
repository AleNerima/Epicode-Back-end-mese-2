using AlbergoApp.Models;

namespace AlbergoApp.Services.Interfaces
{
    public interface IClienteService
    {
        Task<int> CreateClienteAsync(Cliente cliente);
        Task<Cliente?> GetClienteByIdAsync(int idCliente);
        Task<IEnumerable<Cliente>> GetAllClientiAsync();
        Task<bool> UpdateClienteAsync(Cliente cliente);
        Task<bool> DeleteClienteAsync(int idCliente);
        Task<Cliente?> GetClienteByCodiceFiscaleAsync(string codiceFiscale);
    }
}
