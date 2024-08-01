using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;

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
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int[] productIds, int[] quantities, string indirizzoSpedizione, string note)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Controlla che gli array di prodotti e quantità non siano vuoti e abbiano la stessa lunghezza
            if (productIds == null || quantities == null || productIds.Length != quantities.Length)
            {
                ModelState.AddModelError("", "Gli ID dei prodotti e le quantità devono essere validi e corrispondere.");
                var products = await _userService.GetProductsAsync();
                return View("CreateOrder", products);
            }

            try
            {
                var order = await _userService.CreateOrderAsync(productIds, quantities, indirizzoSpedizione, note, userId.Value);
                return RedirectToAction("OrderSummary", new { orderId = order.Id });
            }
            catch (ArgumentException ex)
            {
                // Gestione degli errori
                ModelState.AddModelError("", ex.Message);
                var products = await _userService.GetProductsAsync();
                return View("CreateOrder", products);
            }
            catch (Exception ex)
            {
                // Gestione degli errori generali
                ModelState.AddModelError("", "Si è verificato un errore durante la creazione dell'ordine.");
                var products = await _userService.GetProductsAsync();
                return View("CreateOrder", products);
            }
        }

        public async Task<IActionResult> OrderSummary(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _userService.GetUserOrdersAsync(userId.Value);
            var orderDetail = orders.FirstOrDefault(o => o.Id == orderId);

            if (orderDetail == null)
            {
                return NotFound();
            }

            // Passare i prodotti alla vista per mostrare i dettagli
            var products = await _userService.GetProductsAsync();
            ViewBag.Products = products;

            return View(orderDetail);
        }
    }
}
