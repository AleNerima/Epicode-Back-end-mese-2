using Microsoft.AspNetCore.Mvc;
using PoliziaMunicipaleApp.Services.Interfaces;

namespace PoliziaMunicipaleApp.Controllers
{
    public class VisualizzazioneController : Controller
    {
        private readonly IVisualizzazioneService _visualizzazioneService;

        public VisualizzazioneController(IVisualizzazioneService visualizzazioneService)
        {
            _visualizzazioneService = visualizzazioneService;
        }

        // GET: Visualizzazione/TotaleVerbaliPerTrasgressore
        public async Task<IActionResult> TotaleVerbaliPerTrasgressore()
        {
            var result = await _visualizzazioneService.GetTotaleVerbaliPerTrasgressoreAsync();
            return View(result);
        }

        // GET: Visualizzazione/TotalePuntiDecurtatiPerTrasgressore
        public async Task<IActionResult> TotalePuntiDecurtatiPerTrasgressore()
        {
            var result = await _visualizzazioneService.GetTotalePuntiDecurtatiPerTrasgressoreAsync();
            return View(result);
        }

        // GET: Visualizzazione/ViolazioniConPuntiMaggioreDi10
        public async Task<IActionResult> ViolazioniConPuntiMaggioreDi10()
        {
            var result = await _visualizzazioneService.GetViolazioniConPuntiMaggioreDi10Async();
            return View(result);
        }

        // GET: Visualizzazione/ViolazioniConImportoMaggioreDi400
        public async Task<IActionResult> ViolazioniConImportoMaggioreDi400()
        {
            var result = await _visualizzazioneService.GetViolazioniConImportoMaggioreDi400Async();
            return View(result);
        }
    }
}
