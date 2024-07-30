using System.Threading.Tasks;
using PizzeriaApp.Models;

namespace PizzeriaApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<Product[]> GetProductsAsync();
        Task<Order> CreateOrderAsync(int[] productIds, int[] quantities, string indirizzoSpedizione, string note, int userId);
        Task<Order[]> GetUserOrdersAsync(int userId);
    }
}
