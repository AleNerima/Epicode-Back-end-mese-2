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

        
        public IActionResult Index()
        {
            return View();
        }

    
        public async Task<IActionResult> TotaleVerbaliPerTrasgressore()
        {
            try
            {
                var result = await _visualizzazioneService.GetTotaleVerbaliPerTrasgressoreAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati.";
                return View("Error");
            }
        }

        
        public async Task<IActionResult> TotalePuntiDecurtatiPerTrasgressore()
        {
            try
            {
                var result = await _visualizzazioneService.GetTotalePuntiDecurtatiPerTrasgressoreAsync();
                // Verifica che il risultato non sia null prima di passarne il valore alla vista
                if (result == null)
                {
                    
                    ViewBag.ErrorMessage = "Nessun dato disponibile.";
                    return View("Error");
                }
                return View(result);
            }
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati.";
                return View("Error");
            }
        }


        
        public async Task<IActionResult> ViolazioniConPuntiMaggioreDi10()
        {
            try
            {
                var result = await _visualizzazioneService.GetViolazioniConPuntiMaggioreDi10Async();
                return View(result);
            }
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati.";
                return View("Error");
            }
        }

        
        public async Task<IActionResult> ViolazioniConImportoMaggioreDi400()
        {
            try
            {
                var result = await _visualizzazioneService.GetViolazioniConImportoMaggioreDi400Async();
                return View(result);
            }
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati.";
                return View("Error");
            }
        }
    }
}
