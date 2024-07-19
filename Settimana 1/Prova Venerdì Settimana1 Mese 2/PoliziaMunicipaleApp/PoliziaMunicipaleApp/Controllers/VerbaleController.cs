using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PoliziaMunicipaleApp.Models;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Controllers
{
    public class VerbaleController : Controller
    {
        private readonly IVerbaleService _verbaleService;
        private readonly IAnagraficaService _anagraficaService;
        private readonly ITipoViolazioneService _tipoViolazioneService;

        public VerbaleController(IVerbaleService verbaleService, IAnagraficaService anagraficaService, ITipoViolazioneService tipoViolazioneService)
        {
            _verbaleService = verbaleService;
            _anagraficaService = anagraficaService;
            _tipoViolazioneService = tipoViolazioneService;
        }

        
        public async Task<IActionResult> IndexVerbale()
        {
            var verbali = await _verbaleService.GetAllAsync();
            return View("IndexVerbale", verbali);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var verbale = await _verbaleService.GetByIdAsync(id);
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }

        
        public async Task<IActionResult> CreateVerbale()
        {
            var anagrafiche = await _anagraficaService.GetAllAsync();
            var tipiViolazione = await _tipoViolazioneService.GetAllAsync();

            if (anagrafiche == null || tipiViolazione == null)
            {
                // Gestisci il caso in cui i dati non siano disponibili
                return NotFound();
            }

            ViewData["AnagraficaList"] = new SelectList(anagrafiche, "Idanagrafica", "CodiceFiscale");
            ViewData["TipoViolazioneList"] = new SelectList(tipiViolazione, "Idviolazione", "Descrizione");

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idverbale,DataViolazione,IndirizzoViolazione,NominativoAgente,DataTrascrizioneVerbale,Importo,DecurtamentoPunti,Idanagrafica,Idviolazione")] Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _verbaleService.AddAsync(verbale);
                    return RedirectToAction(nameof(IndexVerbale));
                }
                catch (Exception ex) // Generico, per eventuali errori
                {
                    // Logga o gestisci l'eccezione
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore. Per favore riprova.");
                }
            }

            // Ricarica le liste per la vista in caso di errore
            var anagrafiche = await _anagraficaService.GetAllAsync();
            var tipiViolazione = await _tipoViolazioneService.GetAllAsync();
            ViewData["AnagraficaList"] = new SelectList(anagrafiche, "Idanagrafica", "Nome", verbale.Idanagrafica);
            ViewData["TipoViolazioneList"] = new SelectList(tipiViolazione, "Idviolazione", "Descrizione", verbale.Idviolazione);

            return View(verbale);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var verbale = await _verbaleService.GetByIdAsync(id);
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }

        
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _verbaleService.DeleteAsync(id);
            return RedirectToAction(nameof(IndexVerbale));
        }
    }
}
