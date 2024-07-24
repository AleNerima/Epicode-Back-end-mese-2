using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;


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
            foreach (var prenotazione in prenotazioni)
            {
                // Recupera e aggiungi il CodiceFiscale e Numero
                prenotazione.Cliente = await _clienteService.GetClienteByIdAsync(prenotazione.IdCliente);
                prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.IdCamera);
            }
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

            // Recupera e aggiungi il CodiceFiscale e Numero
            prenotazione.Cliente = await _clienteService.GetClienteByIdAsync(prenotazione.IdCliente);
            prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.IdCamera);

            return View("DetailsPrenotazione", prenotazione);
        }
        

        // GET: Prenotazione/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync(new Prenotazione());
            return View("CreatePrenotazione");
        }

        // POST: Prenotazione/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,IdCamera,DataPrenotazione,NumeroProgressivo,Anno,PeriodoSoggiornoDal,PeriodoSoggiornoAl,CaparraConfirmatoria,Tariffa,TipoSoggiorno,Stato")] Prenotazione prenotazione)
        {
            // Validazione dei vincoli
            if (!IsValidTipoSoggiorno(prenotazione.TipoSoggiorno))
            {
                ModelState.AddModelError("TipoSoggiorno", "Il tipo di soggiorno non è valido.");
            }
            if (!IsValidStato(prenotazione.Stato))
            {
                ModelState.AddModelError("Stato", "Lo stato non è valido.");
            }

            if (ModelState.IsValid)
            {
                await _prenotazioneService.CreatePrenotazioneAsync(prenotazione);
                return RedirectToAction(nameof(Index));
            }

            // Ricarica i dropdown in caso di errore
            await PopulateDropdownsAsync(prenotazione);
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
            await PopulateDropdownsAsync(prenotazione);
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

            // Validazione dei vincoli
            if (!IsValidTipoSoggiorno(prenotazione.TipoSoggiorno))
            {
                ModelState.AddModelError("TipoSoggiorno", "Il tipo di soggiorno non è valido.");
            }
            if (!IsValidStato(prenotazione.Stato))
            {
                ModelState.AddModelError("Stato", "Lo stato non è valido.");
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
            await PopulateDropdownsAsync(prenotazione);
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

        // Metodo per validare TipoSoggiorno
        private bool IsValidTipoSoggiorno(string tipoSoggiorno)
        {
            var validTipoSoggiorni = new[] { "pernottamento con prima colazione", "pensione completa", "mezza pensione" };
            return validTipoSoggiorni.Contains(tipoSoggiorno);
        }

        // Metodo per validare Stato
        private bool IsValidStato(string stato)
        {
            var validStati = new[] { "completata", "cancellata", "confermata" };
            return validStati.Contains(stato);
        }

        // Metodo per popolare i dropdown
        private async Task PopulateDropdownsAsync(Prenotazione prenotazione)
        {
            ViewBag.Clienti = new SelectList(await _clienteService.GetAllClientiAsync(), "IdCliente", "CodiceFiscale", prenotazione.IdCliente);
            ViewBag.Camere = new SelectList(await _cameraService.GetAllCamereAsync(), "IdCamera", "Numero", prenotazione.IdCamera);

            // Popola i dropdown per TipoSoggiorno e Stato
            ViewBag.TipoSoggiorno = new SelectList(new[] {
                "pernottamento con prima colazione",
                "pensione completa",
                "mezza pensione"
            }, prenotazione.TipoSoggiorno);

            ViewBag.Stato = new SelectList(new[] {
                "completata",
                "cancellata",
                "confermata"
            }, prenotazione.Stato);
        }
    }
}
