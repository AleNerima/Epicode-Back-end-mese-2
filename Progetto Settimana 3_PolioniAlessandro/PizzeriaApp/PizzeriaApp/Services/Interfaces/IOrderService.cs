using PizzeriaApp.Models;

namespace PizzeriaApp.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(Ordine ordine);
        Task<List<Ordine>> GetAllOrdersAsync();
        Task<Ordine> GetOrderByIdAsync(int id);
        Task UpdateOrderAsync(Ordine ordine);
    }

}
