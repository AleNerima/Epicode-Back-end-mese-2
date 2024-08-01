using PizzeriaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzeriaApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Order> CreateOrderAsync(int userId, string note, string indirizzoSpedizione);
        Task AddOrderItemAsync(int orderId, int productId, int quantity);
        Task AddOrderItemsAsync(int orderId, int[] productIds, int[] quantities);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetOrderItemsByUserIdAsync(int userId);

    }
}
