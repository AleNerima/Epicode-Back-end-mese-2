using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Services.Interfaces;


namespace PizzeriaApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IImageService _imageService;

        public AdminController(IAdminService adminService, IImageService imageService)
        {
            _adminService = adminService;
            _imageService = imageService;
        }
                
        public async Task<IActionResult> Products()
        {
            var products = await _adminService.GetAllProductsAsync();
            return View(products);
        }
                
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(IFormFile Foto, string Nome, decimal Prezzo, int TempoConsegna, string Ingredienti)
        {
            if (ModelState.IsValid)
            {
                string fotoBase64 = null;

                if (Foto != null && Foto.Length > 0)
                {
                    fotoBase64 = await _imageService.ConvertImageToBase64Async(Foto);
                }

                await _adminService.AddProductAsync(Nome, fotoBase64, Prezzo, TempoConsegna, Ingredienti);
                return RedirectToAction("Products");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _adminService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(int id, IFormFile Foto, string Nome, decimal Prezzo, int TempoConsegna, string Ingredienti)
        {
            // Recupera il prodotto esistente
            var existingProduct = await _adminService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Mantiene l'immagine esistente se non è stato caricato un nuovo file
            string fotoBase64 = existingProduct.Foto;

            if (Foto != null && Foto.Length > 0)
            {
                // Converte il nuovo file in Base64
                fotoBase64 = await _imageService.ConvertImageToBase64Async(Foto);
            }

            // Aggiorna il prodotto con l'immagine esistente o nuova
            await _adminService.UpdateProductAsync(id, Nome, fotoBase64, Prezzo, TempoConsegna, Ingredienti);
            return RedirectToAction("Products");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _adminService.DeleteProductAsync(id);
            return RedirectToAction("Products");
        }

        
        public async Task<IActionResult> OrdersInPreparation()
        {
            var orders = await _adminService.GetOrdersInPreparationAsync();
            var orderDetails = await _adminService.GetAllOrderDetailsAsync();

            var model = orders.Select(order => new
            {
                Order = order,
                Details = orderDetails.Where(od => od.OrderId == order.Id).ToList()
            });

            return View(model);
        }

                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkOrderAsCompleted(int orderId)
        {
            await _adminService.MarkOrderAsCompletedAsync(orderId);
            return RedirectToAction("OrdersInPreparation");
        }

      
        [HttpGet]
        public async Task<IActionResult> GetOrdersByDate(string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                var orders = await _adminService.GetOrdersByDateAsync(parsedDate);
                var totalRevenue = orders.Sum(o => o.OrderItems.Sum(oi => oi.Product.Prezzo * oi.Quantità));

                var model = new
                {
                    Orders = orders.Select(o => new
                    {
                        o.Id,
                        o.OrderDate,
                        o.IndirizzoSpedizione,
                        o.Note
                    }).ToList(),
                    TotalRevenue = totalRevenue
                };

                return Json(model);
            }

            return BadRequest("Data non valida.");
        }


        public async Task<IActionResult> CompletedOrders()
        {
            var orders = await _adminService.GetCompletedOrdersGroupedByDateAsync();
            return View(orders);
        }
    }
}
