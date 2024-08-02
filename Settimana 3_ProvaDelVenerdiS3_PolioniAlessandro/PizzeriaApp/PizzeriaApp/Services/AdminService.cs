using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;


namespace PizzeriaApp.Services
{
    // Implementazione del servizio per l'amministrazione dei prodotti e degli ordini
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        // Costruttore per iniettare il contesto del database
        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ottiene tutti i prodotti dal database
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Ottiene un prodotto specifico per ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // Aggiunge un nuovo prodotto al database
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

        // Aggiorna un prodotto esistente per ID
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

        // Elimina un prodotto dal database per ID
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // Ottiene tutti gli ordini dal database
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        // Ottiene gli ordini in preparazione, inclusi gli articoli degli ordini e i relativi prodotti
        public async Task<IEnumerable<Order>> GetOrdersInPreparationAsync()
        {
            return await _context.Orders
                .Where(o => !o.IsCompleted) // Filtra solo gli ordini non completati
                .Include(o => o.OrderItems) // Include gli OrderItems degli ordini
                .ThenInclude(oi => oi.Product) // Include i prodotti associati agli OrderItems degli ordini
                .ToListAsync();
        }

        // Marca un ordine come completato per ID
        public async Task MarkOrderAsCompletedAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.IsCompleted = true;
                await _context.SaveChangesAsync();
            }
        }

        // Ottiene i dettagli di tutti gli articoli degli ordini
        public async Task<IEnumerable<OrderItemDto>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Product) // Include i dettagli del prodotto
                .Select(oi => new OrderItemDto
                {
                    OrderId = oi.OrderId,
                    ProductName = oi.Product.Nome,
                    Quantity = oi.Quantità
                })
                .ToListAsync();
        }

        // Ottiene gli ordini completati raggruppati per data e calcola il totale degli incassi
        public async Task<IEnumerable<OrderGroupByDateViewModel>> GetCompletedOrdersGroupedByDateAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.IsCompleted) // Filtra solo gli ordini completati
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            var groupedOrders = orders
                .GroupBy(o => o.OrderDate.Date) // Raggruppa gli ordini per data
                .Select(g => new OrderGroupByDateViewModel
                {
                    Date = g.Key,
                    Orders = g.ToList(),
                    TotalRevenue = g.Sum(o => o.OrderItems.Sum(oi => oi.Product.Prezzo * oi.Quantità)) // Calcola il totale degli incassi
                })
                .OrderByDescending(g => g.Date) // Ordina i gruppi per data in modo decrescente
                .ToList();

            return groupedOrders;
        }


        // Ottiene gli ordini per una data specifica
        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date)
        {
            var startOfDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1);

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay) // Filtra gli ordini per la data specificata
                .ToListAsync();

            return orders;
        }

    }
}

