using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;

namespace PizzeriaApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product[]> GetProductsAsync()
        {
            return await _context.Products.ToArrayAsync();
        }

        public async Task<Order> CreateOrderAsync(int[] productIds, int[] quantities, string indirizzoSpedizione, string note, int userId)
        {
            if (productIds == null || quantities == null || productIds.Length != quantities.Length)
            {
                throw new ArgumentException("Il numero di ID prodotto deve corrispondere al numero di quantità.");
            }

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var orderItems = new List<OrderItem>();

            for (int i = 0; i < productIds.Length; i++)
            {
                var productId = productIds[i];
                var quantity = quantities[i];

                if (quantity <= 0)
                {
                    throw new ArgumentException("La quantità deve essere maggiore di zero.");
                }

                if (!products.TryGetValue(productId, out var product))
                {
                    throw new ArgumentException($"Il prodotto con ID {productId} non esiste.");
                }

                var orderItem = new OrderItem
                {
                    ProductId = productId,
                    Quantità = quantity
                };

                orderItems.Add(orderItem);
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Note = note,
                IndirizzoSpedizione = indirizzoSpedizione,
                IsCompleted = false,
                OrderItems = orderItems
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return order;
        }



        public async Task<Order[]> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ToArrayAsync();
        }
    }
}
