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

        // Crea un nuovo ordine per l'utente specificato
        public async Task<Order> CreateOrderAsync(int userId, string note, string indirizzoSpedizione)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now, // Imposta la data dell'ordine a quella corrente
                Note = note,
                IndirizzoSpedizione = indirizzoSpedizione,
                IsCompleted = false // Imposta lo stato dell'ordine come non completato
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Salva le modifiche nel database
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        // Ottiene gli articoli di un ordine specifico tramite l'ID dell'ordine
        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Product) 
                .ToListAsync();
        }

        // Aggiunge un nuovo articolo all'ordine specificato
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

        // Aggiunge più articoli a un ordine in una sola volta
        public async Task AddOrderItemsAsync(int orderId, int[] productIds, int[] quantities)
        {
            for (int i = 0; i < productIds.Length; i++)
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = productIds[i],
                    Quantità = quantities[i] // Quantità di ciascun articolo
                };

                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
        }

        // Ottiene tutti gli articoli degli ordini per un utente specifico
        public async Task<IEnumerable<OrderItem>> GetOrderItemsByUserIdAsync(int userId)
        {
            return await _context.OrderItems
                .Where(oi => oi.Order.UserId == userId)
                .Include(oi => oi.Product) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems) 
                    .ThenInclude(oi => oi.Product) 
                .ToListAsync();
        }

        // Ottiene tutti gli ordini dell'utente che sono stati effettuati oggi o più in là
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Where(o => o.OrderDate.Date >= DateTime.Now.Date) // Filtra per la data odierna o futura
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }



    }
}
