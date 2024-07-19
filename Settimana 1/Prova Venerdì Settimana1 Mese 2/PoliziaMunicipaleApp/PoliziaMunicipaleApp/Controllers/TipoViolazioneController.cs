using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Models;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Controllers
{
    public class TipoViolazioneController : Controller
    {
        private readonly ITipoViolazioneService _tipoViolazioneService;

        public TipoViolazioneController(ITipoViolazioneService tipoViolazioneService)
        {
            _tipoViolazioneService = tipoViolazioneService;
        }

        
        public async Task<IActionResult> IndexTipoViolazione()
        {
            var tipoViolazioni = await _tipoViolazioneService.GetAllAsync();
            return View("IndexTipoViolazione",tipoViolazioni);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var tipoViolazione = await _tipoViolazioneService.GetByIdAsync(id);
            if (tipoViolazione == null)
            {
                return NotFound();
            }
            return View(tipoViolazione);
        }

        
        public IActionResult CreateTipoViolazione()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idviolazione,Descrizione")] TipoViolazione tipoViolazione)
        {
            if (ModelState.IsValid)
            {
                await _tipoViolazioneService.AddAsync(tipoViolazione);
                return RedirectToAction(nameof(IndexTipoViolazione));
            }
            return View(tipoViolazione);
        }

        
        public async Task<IActionResult> EditTipoViolazione(int id)
        {
            var tipoViolazione = await _tipoViolazioneService.GetByIdAsync(id);
            if (tipoViolazione == null)
            {
                return NotFound();
            }
            return View("EditTipoViolazione",tipoViolazione);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTipoViolazione(int id, [Bind("Idviolazione,Descrizione")] TipoViolazione tipoViolazione)
        {
            if (id != tipoViolazione.Idviolazione)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tipoViolazioneService.UpdateAsync(tipoViolazione);
                }
                catch (Exception ex) // Generico, per eventuali errori
                {
                    // Logga o gestisci l'eccezione
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore. Per favore riprova.");
                    return View(tipoViolazione);
                }
                return RedirectToAction(nameof(IndexTipoViolazione));
            }
            return View(tipoViolazione);
        }

        
        public async Task<IActionResult> DeleteTipoViolazione(int id)
        {
            var tipoViolazione = await _tipoViolazioneService.GetByIdAsync(id);
            if (tipoViolazione == null)
            {
                return NotFound();
            }
            return View("DeleteTipoViolazione",tipoViolazione);
        }

        
        [HttpPost, ActionName("DeleteTipoViolazione")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoViolazioneService.DeleteAsync(id);
            return RedirectToAction(nameof(IndexTipoViolazione));
        }
    }
}
