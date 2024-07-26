using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AlbergoApp.Controllers
{
    public class ServizioController : Controller
    {
        private readonly IServizioService _servizioService;
        private readonly IPrenotazioneService _prenotazioneService;

        public ServizioController(IServizioService servizioService, IPrenotazioneService prenotazioneService)
        {
            _servizioService = servizioService;
            _prenotazioneService = prenotazioneService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var servizi = await _servizioService.GetAllServiziAsync();
            return View("IndexServizio", servizi);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View("DetailsServizio", servizio);
        }

        [Authorize (Policy ="AdminOnly")]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> AddToPrenotazione(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }

            // Crea una nuova istanza di ServiziPrenotazione e imposta i valori iniziali
            var model = new ServiziPrenotazione
            {
                IdServizio = servizio.IdServizio,
                Servizio = servizio,
                DataServizio = DateTime.Now,  // Imposta una data predefinita
                Quantita = 1,                // Imposta una quantità predefinita
                PrezzoUnitario = servizio.Prezzo,
                PrezzoTotale = servizio.Prezzo
            };

            // Ottiene tutte le prenotazioni disponibili e vengono passate alla vista tramite ViewBag
            ViewBag.Prenotazioni = await _prenotazioneService.GetAllPrenotazioniAsync();

            return View("AddToPrenotazione", model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToPrenotazione(ServiziPrenotazione model)
        {
            if (ModelState.IsValid)
            {
                var added = await _servizioService.AddServizioToPrenotazioneAsync(
                    model.IdPrenotazione,
                    model.IdServizio,
                    model.DataServizio,
                    model.Quantita,
                    model.PrezzoUnitario
                   );

                if (added)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // In caso di errore, restituisci la vista con il modello corrente e i dati necessari
            ViewBag.Prenotazioni = await _prenotazioneService.GetAllPrenotazioniAsync();
            return View("AddToPrenotazione", model);
        }
    }
}
