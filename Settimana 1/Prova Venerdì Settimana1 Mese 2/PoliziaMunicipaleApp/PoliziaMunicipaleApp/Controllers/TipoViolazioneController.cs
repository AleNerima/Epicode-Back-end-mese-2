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

        // GET: TipoViolazione
        public async Task<IActionResult> Index()
        {
            var tipoViolazioni = await _tipoViolazioneService.GetAllAsync();
            return View(tipoViolazioni);
        }

        // GET: TipoViolazione/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tipoViolazione = await _tipoViolazioneService.GetByIdAsync(id);
            if (tipoViolazione == null)
            {
                return NotFound();
            }
            return View(tipoViolazione);
        }

        // GET: TipoViolazione/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoViolazione/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idviolazione,Descrizione")] TipoViolazione tipoViolazione)
        {
            if (ModelState.IsValid)
            {
                await _tipoViolazioneService.AddAsync(tipoViolazione);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoViolazione);
        }

        // GET: TipoViolazione/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tipoViolazione = await _tipoViolazioneService.GetByIdAsync(id);
            if (tipoViolazione == null)
            {
                return NotFound();
            }
            return View(tipoViolazione);
        }

        // POST: TipoViolazione/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idviolazione,Descrizione")] TipoViolazione tipoViolazione)
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
                return RedirectToAction(nameof(Index));
            }
            return View(tipoViolazione);
        }

        // GET: TipoViolazione/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tipoViolazione = await _tipoViolazioneService.GetByIdAsync(id);
            if (tipoViolazione == null)
            {
                return NotFound();
            }
            return View(tipoViolazione);
        }

        // POST: TipoViolazione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoViolazioneService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
