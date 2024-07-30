using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;

namespace PizzeriaApp.Controllers
{
    //[Authorize]
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

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int[] productIds, int[] quantities, string indirizzoSpedizione, string note)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _userService.CreateOrderAsync(productIds, quantities, indirizzoSpedizione, note, userId.Value);
            return RedirectToAction("OrderSummary", new { orderId = order.Id });
        }

        public async Task<IActionResult> OrderSummary(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _userService.GetUserOrdersAsync(userId.Value);
            var order = orders.FirstOrDefault(o => o.Id == orderId);

            // Passare i prodotti alla vista
            var products = await _userService.GetProductsAsync();
            ViewBag.Products = products;

            return View(order);
        }
    }
}
