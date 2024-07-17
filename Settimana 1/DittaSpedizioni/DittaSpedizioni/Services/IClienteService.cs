using DittaSpedizioni.Models;


namespace DittaSpedizioni.Interfaces
{
    public interface IClienteService
    {
        List<Cliente> GetClienti();
        Cliente GetClienteById(int id);
        void AddCliente(Cliente cliente);
        void UpdateCliente(Cliente cliente);
        void DeleteCliente(int id);
    }
}
