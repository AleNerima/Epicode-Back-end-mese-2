using Microsoft.AspNetCore.Mvc;
using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Models;

namespace DittaSpedizioni.Controllers
{
    public class AggiornamentoSpedizioneController : Controller
    {
        private readonly IAggiornamentoSpedizioneService _aggiornamentoSpedizioneService;

        public AggiornamentoSpedizioneController(IAggiornamentoSpedizioneService aggiornamentoSpedizioneService)
        {
            _aggiornamentoSpedizioneService = aggiornamentoSpedizioneService;
        }

        public IActionResult Index(int spedizioneId)
        {
            var aggiornamenti = _aggiornamentoSpedizioneService.GetAggiornamentiBySpedizioneId(spedizioneId);
            return View(aggiornamenti);
        }

        public IActionResult Create(int spedizioneId)
        {
            var aggiornamento = new AggiornamentoSpedizione { Spedizione = spedizioneId };
            return View(aggiornamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AggiornamentoSpedizione aggiornamento)
        {
            if (ModelState.IsValid)
            {
                _aggiornamentoSpedizioneService.AddAggiornamentoSpedizione(aggiornamento);
                return RedirectToAction(nameof(Index), new { spedizioneId = aggiornamento.Spedizione });
            }
            return View(aggiornamento);
        }
    }
}
