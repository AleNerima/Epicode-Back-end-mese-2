using PizzeriaApp.Data;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(int userId, string note, string indirizzoSpedizione)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Note = note,
                IndirizzoSpedizione = indirizzoSpedizione,
                IsCompleted = false
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Product) // Assicurati di includere il prodotto se necessario
                .ToListAsync();
        }

        public async Task AddOrderItemAsync(int orderId, int productId, int quantity)
        {
            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                Quantità = quantity
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task AddOrderItemsAsync(int orderId, int[] productIds, int[] quantities)
        {
            for (int i = 0; i < productIds.Length; i++)
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = productIds[i],
                    Quantità = quantities[i]
                };

                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByUserIdAsync(int userId)
        {
            return await _context.OrderItems
                .Where(oi => oi.Order.UserId == userId)
                .Include(oi => oi.Product) // Assicurati di includere i dati del prodotto se necessario
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems) // Assicurati di includere gli elementi dell'ordine
                    .ThenInclude(oi => oi.Product) // Assicurati di includere i dati del prodotto
                .ToListAsync();
        }


    }
}
