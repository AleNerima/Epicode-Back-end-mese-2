using PizzeriaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzeriaApp.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(string nome, string foto, decimal prezzo, int tempoConsegna, string ingredienti);
        Task UpdateProductAsync(int id, string nome, string foto, decimal prezzo, int tempoConsegna, string ingredienti);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task MarkOrderAsCompletedAsync(int orderId);
    }
}
