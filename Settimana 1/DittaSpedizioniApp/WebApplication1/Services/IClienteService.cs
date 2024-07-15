using DittaSpedizioniApp.Models;


namespace DittaSpedizioniApp.Services
{
    public interface IClienteService
    {
        List<Cliente> GetClienti();
        Cliente GetClienteById(int id);
        void AggiungiCliente(Cliente cliente);
        void ModificaCliente(Cliente cliente);
        void EliminaCliente(int id);
    }
}
