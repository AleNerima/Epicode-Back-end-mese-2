using PizzeriaApp.Data;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly PizzeriaContextDb _context;

        public OrderService(PizzeriaContextDb context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Ordine ordine)
        {
            _context.Ordini.Add(ordine);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ordine>> GetAllOrdersAsync()
        {
            return await _context.Ordini.Include(o => o.DettagliOrdine)
                                        .ThenInclude(d => d.Prodotto)
                                        .ToListAsync();
        }

        public async Task<Ordine> GetOrderByIdAsync(int id)
        {
            return await _context.Ordini.Include(o => o.DettagliOrdine)
                                        .ThenInclude(d => d.Prodotto)
                                        .FirstOrDefaultAsync(o => o.OrdineId == id);
        }

        public async Task UpdateOrderAsync(Ordine ordine)
        {
            _context.Ordini.Update(ordine);
            await _context.SaveChangesAsync();
        }
    }

}
