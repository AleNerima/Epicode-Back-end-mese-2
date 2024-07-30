using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Services.Interfaces;
using PizzeriaApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

        // GET: /Admin/Products
        public async Task<IActionResult> Products()
        {
            var products = await _adminService.GetAllProductsAsync();
            return View(products);
        }

        // GET: /Admin/AddProduct
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        // POST: /Admin/AddProduct
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

        // GET: /Admin/UpdateProduct/{id}
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
            if (!ModelState.IsValid)
            {
                // Recupera il prodotto esistente se ModelState non è valido
                var product = await _adminService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                // Popola la vista con i dati del prodotto
                product.Nome = Nome;
                product.Prezzo = Prezzo;
                product.TempoConsegna = TempoConsegna;
                product.Ingredienti = Ingredienti;

                return View(product);
            }

            if (Foto == null || Foto.Length == 0)
            {
                ModelState.AddModelError("Foto", "L'immagine è obbligatoria.");
                // Se non è stato fornito un file immagine, recupera il prodotto esistente e mostra errori di validazione
                var product = await _adminService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }

            // Converte l'immagine in Base64
            string fotoBase64 = await _imageService.ConvertImageToBase64Async(Foto);

            // Aggiorna il prodotto
            await _adminService.UpdateProductAsync(id, Nome, fotoBase64, Prezzo, TempoConsegna, Ingredienti);
            return RedirectToAction("Products");
        }




        // POST: /Admin/DeleteProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _adminService.DeleteProductAsync(id);
            return RedirectToAction("Products");
        }

        // GET: /Admin/Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _adminService.GetAllOrdersAsync();
            return View(orders);
        }

        // POST: /Admin/MarkOrderAsCompleted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkOrderAsCompleted(int orderId)
        {
            await _adminService.MarkOrderAsCompletedAsync(orderId);
            return RedirectToAction("Orders");
        }
    }
}
