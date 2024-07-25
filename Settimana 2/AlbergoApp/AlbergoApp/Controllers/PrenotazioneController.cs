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

            // Ottieni i servizi prenotati e converti in lista
            var serviziPrenotati = (await _servizioService.GetServiziPrenotatiByPrenotazioneIdAsync(id)).ToList();

            var model = new PrenotazioneDettagliViewModel
            {
                Prenotazione = prenotazione,
                ServiziPrenotati = serviziPrenotati
            };

            return View("DettagliServiziPrenotazione", model);
        }
    }
}
