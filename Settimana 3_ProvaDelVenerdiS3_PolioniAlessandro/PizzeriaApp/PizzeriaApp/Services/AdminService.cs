using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;


namespace PizzeriaApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddProductAsync(string nome, string foto, decimal prezzo, int tempoConsegna, string ingredienti)
        {
            var product = new Product
            {
                Nome = nome,
                Foto = foto,
                Prezzo = prezzo,
                TempoConsegna = tempoConsegna,
                Ingredienti = ingredienti
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(int id, string nome, string foto, decimal prezzo, int tempoConsegna, string ingredienti)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                product.Nome = nome;
                product.Foto = foto;
                product.Prezzo = prezzo;
                product.TempoConsegna = tempoConsegna;
                product.Ingredienti = ingredienti;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersInPreparationAsync()
        {
            return await _context.Orders
                .Where(o => !o.IsCompleted)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task MarkOrderAsCompletedAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.IsCompleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Select(oi => new OrderItemDto
                {
                    OrderId = oi.OrderId,
                    ProductName = oi.Product.Nome,
                    Quantity = oi.Quantità
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderGroupByDateViewModel>> GetCompletedOrdersGroupedByDateAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.IsCompleted)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            var groupedOrders = orders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new OrderGroupByDateViewModel
                {
                    Date = g.Key,
                    Orders = g.ToList(),
                    TotalRevenue = g.Sum(o => o.OrderItems.Sum(oi => oi.Product.Prezzo * oi.Quantità))
                })
                .OrderByDescending(g => g.Date)
                .ToList();

            return groupedOrders;
        }
        


        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date)
        {
            var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1);

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay)
                .ToListAsync();

            return orders;
        }

    }
}

