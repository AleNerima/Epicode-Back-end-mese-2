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
            var orderItems = productIds.Select((productId, index) => new OrderItem
            {
                ProductId = productId,
                Quantità = quantities[index]
            }).ToList();

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Note = note,
                IndirizzoSpedizione = indirizzoSpedizione,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

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
