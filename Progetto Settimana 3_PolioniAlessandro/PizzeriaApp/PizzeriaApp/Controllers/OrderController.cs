using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Services.Interfaces;

namespace PizzeriaApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ordine = await _orderService.GetOrderByIdAsync(id);
            if (ordine == null)
            {
                return NotFound();
            }
            return View(ordine);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var ordine = await _orderService.GetOrderByIdAsync(id);
            if (ordine == null)
            {
                return NotFound();
            }
            ordine.Evaso = true;
            await _orderService.UpdateOrderAsync(ordine);
            return RedirectToAction("Index");
        }
    }

}
