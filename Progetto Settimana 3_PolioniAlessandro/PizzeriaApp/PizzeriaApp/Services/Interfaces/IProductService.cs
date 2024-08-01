using PizzeriaApp.Models;

namespace PizzeriaApp.Services.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Prodotto prodotto);
        Task<List<Prodotto>> GetAllProductsAsync();
        Task<Prodotto> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Prodotto prodotto);
        Task DeleteProductAsync(int id);
    }

}
