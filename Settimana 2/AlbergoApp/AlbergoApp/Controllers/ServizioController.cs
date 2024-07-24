using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using System.Threading.Tasks;

namespace AlbergoApp.Controllers
{
    public class ServizioController : Controller
    {
        private readonly IServizioService _servizioService;

        public ServizioController(IServizioService servizioService)
        {
            _servizioService = servizioService;
        }

        
        public async Task<IActionResult> Index()
        {
            var servizi = await _servizioService.GetAllServiziAsync();
            return View("IndexServizio", servizi);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View("DetailsServizio", servizio);
        }

        
        public IActionResult Create()
        {
            return View("CreateServizio");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeServizio, Prezzo")] Servizio servizio)
        {
            if (ModelState.IsValid)
            {
                await _servizioService.CreateServizioAsync(servizio);
                return RedirectToAction(nameof(Index));
            }
            return View("CreateServizio", servizio);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View("EditServizio", servizio);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdServizio, NomeServizio, Prezzo")] Servizio servizio)
        {
            if (id != servizio.IdServizio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = await _servizioService.UpdateServizioAsync(servizio);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View("EditServizio", servizio);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View("DeleteServizio", servizio);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _servizioService.DeleteServizioAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
