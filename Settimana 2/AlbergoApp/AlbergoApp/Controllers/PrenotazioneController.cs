using Microsoft.AspNetCore.Mvc;
using AlbergoApp.Models;
using AlbergoApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;


namespace AlbergoApp.Controllers
{
    public class PrenotazioneController : Controller
    {
        private readonly IPrenotazioneService _prenotazioneService;
        private readonly IClienteService _clienteService;
        private readonly ICameraService _cameraService;
        private readonly IServizioService _servizioService;

        public PrenotazioneController(IPrenotazioneService prenotazioneService, IClienteService clienteService, ICameraService cameraService, IServizioService servizioService)
        {
            _prenotazioneService = prenotazioneService;
            _clienteService = clienteService;
            _cameraService = cameraService;
            _servizioService = servizioService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var prenotazioni = await _prenotazioneService.GetAllPrenotazioniAsync();
            foreach (var prenotazione in prenotazioni)
            {
                prenotazione.Cliente = await _clienteService.GetClienteByIdAsync(prenotazione.IdCliente);
                prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.IdCamera);
            }
            return View("IndexPrenotazione", prenotazioni);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _prenotazioneService.SearchPrenotazioniAsync(query);
            return Json(results.Select(prenotazione => new
            {
                idPrenotazione = prenotazione.IdPrenotazione,
                numeroProgressivo= prenotazione.NumeroProgressivo,
                codiceFiscaleCliente = prenotazione.Cliente?.CodiceFiscale,
                numeroCamera = prenotazione.Camera?.Numero,
                dataPrenotazione = prenotazione.DataPrenotazione.ToShortDateString(),
                periodoSoggiorno = "Dal" + prenotazione.PeriodoSoggiornoDal.ToShortDateString() + " Al " + prenotazione.PeriodoSoggiornoAl.ToShortDateString(),
                caparraConfirmatoria = prenotazione.CaparraConfirmatoria.ToString("C"),
                tariffa = prenotazione.Tariffa.ToString("C"),
                tipoSoggiorno = prenotazione.TipoSoggiorno,
                stato = prenotazione.Stato
            }));
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            prenotazione.Cliente = await _clienteService.GetClienteByIdAsync(prenotazione.IdCliente);
            prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.IdCamera);

            return View("DetailsPrenotazione", prenotazione);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync(new Prenotazione());
            return View("CreatePrenotazione");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,IdCamera,DataPrenotazione,NumeroProgressivo,Anno,PeriodoSoggiornoDal,PeriodoSoggiornoAl,CaparraConfirmatoria,Tariffa,TipoSoggiorno,Stato")] Prenotazione prenotazione)
        {
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
                var camereDisponibili = await _cameraService.GetAvailableCamereAsync(prenotazione.PeriodoSoggiornoDal, prenotazione.PeriodoSoggiornoAl);
                if (!camereDisponibili.Any(c => c.IdCamera == prenotazione.IdCamera))
                {
                    ModelState.AddModelError("IdCamera", "La camera selezionata non è disponibile per il periodo specificato.");
                }
            }

            if (ModelState.IsValid)
            {
                await _prenotazioneService.CreatePrenotazioneAsync(prenotazione);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(prenotazione);
            return View("CreatePrenotazione", prenotazione);
        }

        [Authorize]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPrenotazione,IdCliente,IdCamera,DataPrenotazione,NumeroProgressivo,Anno,PeriodoSoggiornoDal,PeriodoSoggiornoAl,CaparraConfirmatoria,Tariffa,TipoSoggiorno,Stato")] Prenotazione prenotazione)
        {
            if (id != prenotazione.IdPrenotazione)
            {
                return NotFound();
            }

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
                var camereDisponibili = await _cameraService.GetAvailableCamereAsync(prenotazione.PeriodoSoggiornoDal, prenotazione.PeriodoSoggiornoAl);
                if (!camereDisponibili.Any(c => c.IdCamera == prenotazione.IdCamera))
                {
                    ModelState.AddModelError("IdCamera", "La camera selezionata non è disponibile per il periodo specificato.");
                }
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

            await PopulateDropdownsAsync(prenotazione);
            return View("EditPrenotazione", prenotazione);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View("DeletePrenotazione", prenotazione);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _prenotazioneService.DeletePrenotazioneAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool IsValidTipoSoggiorno(string tipoSoggiorno)
        {
            var validTipoSoggiorni = new[] { "pernottamento con prima colazione", "pensione completa", "mezza pensione" };
            return validTipoSoggiorni.Contains(tipoSoggiorno);
        }

        private bool IsValidStato(string stato)
        {
            var validStati = new[] { "completata", "cancellata", "confermata" };
            return validStati.Contains(stato);
        }

        private async Task PopulateDropdownsAsync(Prenotazione prenotazione)
        {
            ViewBag.Clienti = new SelectList(await _clienteService.GetAllClientiAsync(), "IdCliente", "CodiceFiscale", prenotazione.IdCliente);
            ViewBag.Camere = new SelectList(await _cameraService.GetAllCamereAsync(), "IdCamera", "Numero", prenotazione.IdCamera);

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

        [Authorize]
        public async Task<IActionResult> DettagliServizi(int id)
        {
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            prenotazione.Cliente = await _clienteService.GetClienteByIdAsync(prenotazione.IdCliente);
            prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.IdCamera);

            // Ottieni i servizi prenotati e converte in lista
            var serviziPrenotati = (await _servizioService.GetServiziPrenotatiByPrenotazioneIdAsync(id)).ToList();

            var model = new PrenotazioneDettagliViewModel
            {
                Prenotazione = prenotazione,
                ServiziPrenotati = serviziPrenotati
            };

            return View("DettagliServiziPrenotazione", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAvailableCamere(DateTime startDate, DateTime endDate)
        {
            // Verifico che le date siano valide
            if (startDate >= endDate)
            {
                return BadRequest("La data di inizio deve essere prima della data di fine.");
            }

            var camereDisponibili = await _cameraService.GetAvailableCamereAsync(startDate, endDate);

            // Restituisce le stanze disponibili come JSON
            return Json(camereDisponibili.Select(camera => new
            {
                idCamera = camera.IdCamera,
                numero = camera.Numero,
                descrizione = camera.Descrizione
            }));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Checkout(int id)
        {
            // Recupero la prenotazione e i servizi associati
            var prenotazione = await _prenotazioneService.GetPrenotazioneByIdAsync(id);
            if (prenotazione == null)
            {
                return Json(new { error = "Prenotazione non trovata" });
            }

            var serviziPrenotati = (await _servizioService.GetServiziPrenotatiByPrenotazioneIdAsync(id)).ToList();

            // Calcolo il totale dei servizi
            decimal totaleServizi = serviziPrenotati.Sum(s => s.Quantita * s.PrezzoUnitario);

            // Calcolo il totale da pagare
            decimal totale = prenotazione.Tariffa - prenotazione.CaparraConfirmatoria + totaleServizi;

            // Preparo i dettagli per la visualizzazione
            var dettagli = serviziPrenotati.Select(s =>
                $"{s.Servizio.NomeServizio}: {s.Quantita} x {s.PrezzoUnitario.ToString("C")} = {(s.Quantita * s.PrezzoUnitario).ToString("C")}"
            ).ToList();
            dettagli.Insert(0, $"Tariffa: {prenotazione.Tariffa.ToString("C")}");
            dettagli.Insert(1, $"Caparra Confirmatoria: {prenotazione.CaparraConfirmatoria.ToString("C")}");
            dettagli.Add($"Totale Servizi: {totaleServizi.ToString("C")}");
            dettagli.Add($"Totale da Pagare: {totale.ToString("C")}");

            // Restituisce i dati come JSON
            return Json(new { totale = totale.ToString("C"), dettagli = string.Join("\n", dettagli) });
        }


    }
}
