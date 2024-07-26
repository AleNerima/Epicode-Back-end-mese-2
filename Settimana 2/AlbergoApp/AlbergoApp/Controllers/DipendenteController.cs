using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace AlbergoApp.Controllers
{
    public class DipendenteController : Controller
    {
        private readonly IDipendenteService _dipendenteService;

        public DipendenteController(IDipendenteService dipendenteService)
        {
            _dipendenteService = dipendenteService;
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
            var dipendenti = await _dipendenteService.GetAllDipendentiAsync();
            return View("IndexDipendente", dipendenti);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Details(int id)
        {
            var dipendente = await _dipendenteService.GetDipendenteByIdAsync(id);
            if (dipendente == null)
            {
                return NotFound();
            }
            return View("DetailsDipendente", dipendente);
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View("CreateDipendente");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username, PasswordHash, Nome, Cognome, Ruolo")] Dipendente dipendente)
        {
            if (ModelState.IsValid)
            {
                
                int id = await _dipendenteService.CreateDipendenteAsync(dipendente);
                return RedirectToAction(nameof(Index));
            }
            return View("CreateDipendente", dipendente);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id)
        {
            var dipendente = await _dipendenteService.GetDipendenteByIdAsync(id);
            if (dipendente == null)
            {
                return NotFound();
            }
            return View("EditDipendente", dipendente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDipendente, Username, PasswordHash, Nome, Cognome, Ruolo")] Dipendente dipendente)
        {
            if (id != dipendente.IdDipendente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = await _dipendenteService.UpdateDipendenteAsync(dipendente);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View("EditDipendente", dipendente);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var dipendente = await _dipendenteService.GetDipendenteByIdAsync(id);
            if (dipendente == null)
            {
                return NotFound();
            }
            return View("DeleteDipendente", dipendente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dipendenteService.DeleteDipendenteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
