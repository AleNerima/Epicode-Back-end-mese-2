using PizzeriaApp.Models;


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

        Task<IEnumerable<Order>> GetOrdersInPreparationAsync();

        Task<IEnumerable<OrderGroupByDateViewModel>> GetCompletedOrdersGroupedByDateAsync();

        Task<IEnumerable<OrderItemDto>> GetAllOrderDetailsAsync();
        Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date);
    }
}
