using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Models;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Controllers
{
    public class AnagraficaController : Controller
    {
        private readonly IAnagraficaService _anagraficaService;

        public AnagraficaController(IAnagraficaService anagraficaService)
        {
            _anagraficaService = anagraficaService;
        }

        public async Task<IActionResult> IndexAnagrafica()
        {
            try
            {
                var anagrafiche = await _anagraficaService.GetAllAsync();
                return View("IndexAnagrafica",anagrafiche);
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (potresti usare un logger qui)
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati.";
                return View("Error");
            }
        }

        public async Task<IActionResult> DetailsAnagrafica(int id)
        {
            try
            {
                var anagrafica = await _anagraficaService.GetByIdAsync(id);
                if (anagrafica == null)
                {
                    return NotFound();
                }
                return View("DetailsAnagrafica",anagrafica);
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (potresti usare un logger qui)
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dettagli.";
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cognome,Nome,Indirizzo,Citta,CAP,CodiceFiscale")] Anagrafica anagrafica)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _anagraficaService.AddAsync(anagrafica);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log dell'eccezione (potresti usare un logger qui)
                    ViewBag.ErrorMessage = "Si è verificato un errore durante l'inserimento dei dati.";
                    return View("Error");
                }
            }
            return View(anagrafica);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var anagrafica = await _anagraficaService.GetByIdAsync(id);
                if (anagrafica == null)
                {
                    return NotFound();
                }
                return View(anagrafica);
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (potresti usare un logger qui)
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati per la modifica.";
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idanagrafica,Cognome,Nome,Indirizzo,Citta,CAP,CodiceFiscale")] Anagrafica anagrafica)
        {
            if (id != anagrafica.Idanagrafica)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _anagraficaService.UpdateAsync(anagrafica);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log dell'eccezione (potresti usare un logger qui)
                    ViewBag.ErrorMessage = "Si è verificato un errore durante l'aggiornamento dei dati.";
                    return View("Error");
                }
            }
            return View(anagrafica);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var anagrafica = await _anagraficaService.GetByIdAsync(id);
                if (anagrafica == null)
                {
                    return NotFound();
                }
                return View(anagrafica);
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (potresti usare un logger qui)
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati per l'eliminazione.";
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _anagraficaService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (potresti usare un logger qui)
                ViewBag.ErrorMessage = "Si è verificato un errore durante l'eliminazione dei dati.";
                return View("Error");
            }
        }
    }
}
