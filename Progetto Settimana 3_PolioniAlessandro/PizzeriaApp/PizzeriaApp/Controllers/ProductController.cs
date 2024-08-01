using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Models;
using PizzeriaApp.Services.Interfaces;

namespace PizzeriaApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly PizzeriaContextDb _context;

        public ProductController(IProductService productService, PizzeriaContextDb context)
        {
            _productService = productService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nome, IFormFile foto, decimal prezzo, int tempoConsegna, string ingredienti)
        {
            if (foto != null && foto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await foto.CopyToAsync(memoryStream);
                    var fotoBase64 = Convert.ToBase64String(memoryStream.ToArray());

                    var prodotto = new Prodotto
                    {
                        Nome = nome,
                        FotoBase64 = fotoBase64,
                        Prezzo = prezzo,
                        TempoConsegna = tempoConsegna,
                        Ingredienti = ingredienti
                    };

                    _context.Prodotti.Add(prodotto);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Foto non valida.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var prodotto = await _context.Prodotti.FindAsync(id);

            if (prodotto == null)
            {
                return NotFound();
            }

            return View(prodotto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string nome, IFormFile foto, decimal prezzo, int tempoConsegna, string ingredienti)
        {
            var prodotto = await _context.Prodotti.FindAsync(id);

            if (prodotto == null)
            {
                return NotFound();
            }

            if (foto != null && foto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await foto.CopyToAsync(memoryStream);
                    prodotto.FotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            prodotto.Nome = nome;
            prodotto.Prezzo = prezzo;
            prodotto.TempoConsegna = tempoConsegna;
            prodotto.Ingredienti = ingredienti;

            _context.Update(prodotto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var prodotto = await _context.Prodotti
                .FirstOrDefaultAsync(p => p.ProdottoId == id);

            if (prodotto == null)
            {
                return NotFound();
            }

            return View(prodotto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var prodotto = await _productService.GetProductByIdAsync(id);
            if (prodotto == null)
            {
                return NotFound();
            }
            return View(prodotto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }
    }

}
