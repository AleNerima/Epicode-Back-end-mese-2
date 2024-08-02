using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Services.Interfaces;
using PizzeriaApp.Models;
using System.Security.Claims;


namespace PizzeriaApp.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Products()
        {
            var products = await _userService.GetProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var products = await _userService.GetProductsAsync();
            var model = new CreateOrderViewModel
            {
                Products = products
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(
            string indirizzoSpedizione,
            string note,
            int[] productIds,
            int[] quantities)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int userIdInt))
            {
                ModelState.AddModelError("", "Utente non autenticato o ID utente non valido.");
                return RedirectToAction("Login", "Account");
            }

            // Verifica che gli array dei prodotti e delle quantità siano validi
            if (productIds == null || quantities == null || productIds.Length != quantities.Length)
            {
                ModelState.AddModelError("", "Dati invalidi per i prodotti o le quantità.");
                var products = await _userService.GetProductsAsync();
                var model = new CreateOrderViewModel
                {
                    Products = products
                };
                return View(model);
            }

            // Rimuove i prodotti con quantità non positiva
            var validItems = productIds
                .Select((id, index) => new { ProductId = id, Quantity = quantities[index] })
                .Where(item => item.Quantity > 0)
                .ToList();

            if (!validItems.Any())
            {
                ModelState.AddModelError("", "Devi selezionare almeno un prodotto con una quantità positiva.");
                var products = await _userService.GetProductsAsync();
                var model = new CreateOrderViewModel
                {
                    Products = products
                };
                return View(model);
            }

            try
            {
                var order = await _userService.CreateOrderAsync(userIdInt, note, indirizzoSpedizione);

                if (order == null)
                {
                    ModelState.AddModelError("", "Impossibile creare l'ordine.");
                    var products = await _userService.GetProductsAsync();
                    var model = new CreateOrderViewModel
                    {
                        Products = products
                    };
                    return View(model);
                }

                foreach (var item in validItems)
                {
                    await _userService.AddOrderItemAsync(order.Id, item.ProductId, item.Quantity);
                }

                return RedirectToAction("OrderSummary", new { orderId = order.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore durante la creazione dell'ordine: {ex.Message}");
                var products = await _userService.GetProductsAsync();
                var model = new CreateOrderViewModel
                {
                    Products = products
                };
                return View(model);
            }
        }

        public async Task<IActionResult> OrderSummary(int orderId)
        {
            var order = await _userService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound(); 
            }

            var orderItems = await _userService.GetOrderItemsByOrderIdAsync(orderId);
            var orderItemViewModels = orderItems.Select(item => new OrderItemViewModel
            {
                Product = item.Product,
                Quantità = item.Quantità
            }).ToList();

            var model = new OrderSummaryViewModel
            {
                Order = order,
                OrderItems = orderItemViewModels 
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalPrice(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("ID dell'ordine non valido.");
            }

            try
            {
                // Recupera gli articoli dell'ordine
                var orderItems = await _userService.GetOrderItemsByOrderIdAsync(orderId);

                if (orderItems == null || !orderItems.Any())
                {
                    return BadRequest("Ordine non trovato.");
                }

                // Calcola il prezzo totale
                var totalPrice = orderItems.Sum(item => item.Product.Prezzo * item.Quantità);

                // Restituisce il prezzo totale come JSON
                return Json(new { totalPrice });
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Errore: {ex.Message}");
                return StatusCode(500, "Errore interno del server.");
            }
        }

        public async Task<IActionResult> MyOrders()
        {
            // Recupera l'ID utente dai claim
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                // Gestisce il caso in cui l'ID utente non è disponibile 
                return RedirectToAction("Login", "Account");
            }

            // Recupera gli ordini dell'utente
            var orders = await _userService.GetUserOrdersAsync(userId);

            var currentDate = DateTime.Now.Date;
            var filteredOrders = orders
                .Where(o => o.OrderDate.Date == currentDate)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.IndirizzoSpedizione,
                    o.Note,
                    Status = o.IsCompleted ? "In Consegna" : "In Preparazione",
                    Items = o.OrderItems.Select(oi => new
                    {
                        oi.Product.Nome,
                        oi.Quantità
                    }).ToList()
                }).ToList();

            return View(filteredOrders);
        }

    }
}

