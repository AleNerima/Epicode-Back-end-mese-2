using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;

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

        // GET: Servizio
        public async Task<IActionResult> Index()
        {
            var servizi = await _servizioService.GetAllServiziAsync();
            return View("IndexServizio", servizi);
        }

        // GET: Servizio/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View("DetailsServizio", servizio);
        }

        // GET: Servizio/Create
        public IActionResult Create()
        {
            return View("CreateServizio");
        }

        // POST: Servizio/Create
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

        // GET: Servizio/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View("EditServizio", servizio);
        }

        // POST: Servizio/Edit/5
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

        // GET: Servizio/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var servizio = await _servizioService.GetServizioByIdAsync(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View("DeleteServizio", servizio);
        }

        // POST: Servizio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _servizioService.DeleteServizioAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Servizio/AddToPrenotazione/5
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

            // Ottieni tutte le prenotazioni disponibili e passale alla vista tramite ViewBag
            ViewBag.Prenotazioni = await _prenotazioneService.GetAllPrenotazioniAsync();

            return View("AddToPrenotazione", model);
        }

        // POST: Servizio/AddToPrenotazione
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
