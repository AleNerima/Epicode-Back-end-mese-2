using PizzeriaApp.Data;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.Services
{
    public class ProductService : IProductService
    {
        private readonly PizzeriaContextDb _context;

        public ProductService(PizzeriaContextDb context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Prodotto prodotto)
        {
            _context.Prodotti.Add(prodotto);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Prodotto>> GetAllProductsAsync()
        {
            return await _context.Prodotti.ToListAsync();
        }

        public async Task<Prodotto> GetProductByIdAsync(int id)
        {
            return await _context.Prodotti.FindAsync(id);
        }

        public async Task UpdateProductAsync(Prodotto prodotto)
        {
            _context.Prodotti.Update(prodotto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var prodotto = await _context.Prodotti.FindAsync(id);
            if (prodotto != null)
            {
                _context.Prodotti.Remove(prodotto);
                await _context.SaveChangesAsync();
            }
        }
    }

}
