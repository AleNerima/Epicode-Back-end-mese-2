using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace AlbergoApp.Controllers
{
    public class PrenotazioneController : Controller
    {
        private readonly IPrenotazioneService _prenotazioneService;
        private readonly IClienteService _clienteService;
        private readonly ICameraService _cameraService;

        public PrenotazioneController(IPrenotazioneService prenotazioneService, IClienteService clienteService, ICameraService cameraService)
        {
            _prenotazioneService = prenotazioneService;
            _clienteService = clienteService;
            _cameraService = cameraService;
        }

        // GET: Prenotazione
        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneService.GetAllPrenotazioniAsync();
            return View("IndexPrenotazione", prenotazioni);
        }

        // GET: Prenotazione/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View("DetailsPrenotazione", prenotazione);
        }

        // GET: Prenotazione/Create
        public async Task<IActionResult> Create()
        {
            // Popola i dropdown con ViewBag
            ViewBag.Clienti = new SelectList(await _clienteService.GetAllClientiAsync(), "IdCliente", "CodiceFiscale");
            ViewBag.Camere = new SelectList(await _cameraService.GetAllCamereAsync(), "IdCamera", "Numero");

            return View("CreatePrenotazione");
        }

        // POST: Prenotazione/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,IdCamera,DataPrenotazione,NumeroProgressivo,Anno,PeriodoSoggiornoDal,PeriodoSoggiornoAl,CaparraConfirmatoria,Tariffa,TipoSoggiorno,Stato")] Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                await _prenotazioneService.CreatePrenotazioneAsync(prenotazione);
                return RedirectToAction(nameof(Index));
            }

            // Ricarica i dropdown in caso di errore
            ViewBag.Clienti = new SelectList(await _clienteService.GetAllClientiAsync(), "IdCliente", "CodiceFiscale", prenotazione.IdCliente);
            ViewBag.Camere = new SelectList(await _cameraService.GetAllCamereAsync(), "IdCamera", "Numero", prenotazione.IdCamera);

            return View("CreatePrenotazione", prenotazione);
        }

        // GET: Prenotazione/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            // Popola i dropdown
            ViewBag.Clienti = new SelectList(await _clienteService.GetAllClientiAsync(), "IdCliente", "CodiceFiscale", prenotazione.IdCliente);
            ViewBag.Camere = new SelectList(await _cameraService.GetAllCamereAsync(), "IdCamera", "Numero", prenotazione.IdCamera);

            return View("EditPrenotazione", prenotazione);
        }

        // POST: Prenotazione/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPrenotazione,IdCliente,IdCamera,DataPrenotazione,NumeroProgressivo,Anno,PeriodoSoggiornoDal,PeriodoSoggiornoAl,CaparraConfirmatoria,Tariffa,TipoSoggiorno,Stato")] Prenotazione prenotazione)
        {
            if (id != prenotazione.IdPrenotazione)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = await _prenotazioneService.UpdatePrenotazioneAsync(prenotazione);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            // Ricarica i dropdown in caso di errore
            ViewBag.Clienti = new SelectList(await _clienteService.GetAllClientiAsync(), "IdCliente", "CodiceFiscale", prenotazione.IdCliente);
            ViewBag.Camere = new SelectList(await _cameraService.GetAllCamereAsync(), "IdCamera", "Numero", prenotazione.IdCamera);

            return View("EditPrenotazione", prenotazione);
        }

        // GET: Prenotazione/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View("DeletePrenotazione", prenotazione);
        }

        // POST: Prenotazione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _prenotazioneService.DeletePrenotazioneAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
