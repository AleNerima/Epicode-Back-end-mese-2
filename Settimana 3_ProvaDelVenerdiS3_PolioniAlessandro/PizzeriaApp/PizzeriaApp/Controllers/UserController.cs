using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Services.Interfaces;
using PizzeriaApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;

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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

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

            // Debug: Stampa tutti i claims
            Console.WriteLine("Claims dell'utente:");
            foreach (var claim in HttpContext.User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            // Debug: Log dell'ID dell'utente
            Console.WriteLine($"User ID: {userId}");

            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User ID non trovato. Utente non autenticato.");
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                Console.WriteLine("User ID non valido. Impossibile convertire in int.");
                return RedirectToAction("Login", "Account");
            }

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

            try
            {
                var order = await _userService.CreateOrderAsync(userIdInt, note, indirizzoSpedizione);

                if (order == null)
                {
                    Console.WriteLine("Impossibile creare l'ordine.");
                    ModelState.AddModelError("", "Impossibile creare l'ordine.");
                    var products = await _userService.GetProductsAsync();
                    var model = new CreateOrderViewModel
                    {
                        Products = products
                    };
                    return View(model);
                }

                Console.WriteLine($"Ordine creato con ID: {order.Id}");

                for (int i = 0; i < productIds.Length; i++)
                {
                    if (quantities[i] > 0)
                    {
                        await _userService.AddOrderItemAsync(order.Id, productIds[i], quantities[i]);
                    }
                }

                return RedirectToAction("OrderSummary", new { orderId = order.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la creazione dell'ordine: {ex.Message}");
                ModelState.AddModelError("", "Si è verificato un errore durante la creazione dell'ordine.");
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
                return NotFound(); // O una vista di errore personalizzata
            }

            var orderItems = await _userService.GetOrderItemsByOrderIdAsync(orderId);
            var model = new OrderSummaryViewModel
            {
                Order = order,
                OrderItems = orderItems
            };

            return View(model);
        }
    }
}
