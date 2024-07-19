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

        // GET: Verbale
        public async Task<IActionResult> Index()
        {
            var verbali = await _verbaleService.GetAllAsync();
            return View(verbali);
        }

        // GET: Verbale/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var verbale = await _verbaleService.GetByIdAsync(id);
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }

        // GET: Verbale/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AnagraficaList"] = new SelectList(await _anagraficaService.GetAllAsync(), "Idanagrafica", "Nome");
            ViewData["TipoViolazioneList"] = new SelectList(await _tipoViolazioneService.GetAllAsync(), "Idviolazione", "Descrizione");
            return View();
        }

        // POST: Verbale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idverbale,DataViolazione,IndirizzoViolazione,NominativoAgente,DataTrascrizioneVerbale,Importo,DecurtamentoPunti,Idanagrafica,Idviolazione")] Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _verbaleService.AddAsync(verbale);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) // Generico, per eventuali errori
                {
                    // Logga o gestisci l'eccezione
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore. Per favore riprova.");
                }
            }

            ViewData["AnagraficaList"] = new SelectList(await _anagraficaService.GetAllAsync(), "Idanagrafica", "Nome", verbale.Idanagrafica);
            ViewData["TipoViolazioneList"] = new SelectList(await _tipoViolazioneService.GetAllAsync(), "Idviolazione", "Descrizione", verbale.Idviolazione);
            return View(verbale);
        }

        // GET: Verbale/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var verbale = await _verbaleService.GetByIdAsync(id);
            if (verbale == null)
            {
                return NotFound();
            }
            ViewData["AnagraficaList"] = new SelectList(await _anagraficaService.GetAllAsync(), "Idanagrafica", "Nome", verbale.Idanagrafica);
            ViewData["TipoViolazioneList"] = new SelectList(await _tipoViolazioneService.GetAllAsync(), "Idviolazione", "Descrizione", verbale.Idviolazione);
            return View(verbale);
        }

        // POST: Verbale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idverbale,DataViolazione,IndirizzoViolazione,NominativoAgente,DataTrascrizioneVerbale,Importo,DecurtamentoPunti,Idanagrafica,Idviolazione")] Verbale verbale)
        {
            if (id != verbale.Idverbale)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _verbaleService.UpdateAsync(verbale);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) // Generico, per eventuali errori
                {
                    // Logga o gestisci l'eccezione
                    ModelState.AddModelError(string.Empty, "Si è verificato un errore. Per favore riprova.");
                }
            }

            ViewData["AnagraficaList"] = new SelectList(await _anagraficaService.GetAllAsync(), "Idanagrafica", "Nome", verbale.Idanagrafica);
            ViewData["TipoViolazioneList"] = new SelectList(await _tipoViolazioneService.GetAllAsync(), "Idviolazione", "Descrizione", verbale.Idviolazione);
            return View(verbale);
        }

        // GET: Verbale/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var verbale = await _verbaleService.GetByIdAsync(id);
            if (verbale == null)
            {
                return NotFound();
            }
            return View(verbale);
        }

        // POST: Verbale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _verbaleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
